using business_foundation_lib.xq_thriftlib_config;
using IDLAutoGenerated.Util;
using lib.xqclient_base.logger;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;


namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(IUserSettingSyncController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserSettingSyncController : IUserSettingSyncController, IXueQiaoFoundationSingletonController
    {
        class UserSettingItemKey : Tuple<string, string>
        {
            public UserSettingItemKey(string settingKey, string userToken) : base(settingKey, userToken)
            {
                this.SettingKey = settingKey;
                this.UserToken = userToken;
            }

            public readonly string SettingKey;
            public readonly string UserToken;
        }

        class UserSettingHolderItem
        {
            public UserSettingHolderItem(UserSettingItemKey itemKey)
            {
                this.ItemKey = itemKey;
            }

            public readonly UserSettingItemKey ItemKey;

            public int? LastSettingVersion;
            public string LastSettingContent;
        }

        
        private readonly Lazy<ILoginUserManageService> loginUserManageService;

        private readonly Dictionary<UserSettingItemKey, UserSettingHolderItem> userSettingItems = new Dictionary<UserSettingItemKey, UserSettingHolderItem>();
        private readonly object userSettingItemsLock = new object();

        [ImportingConstructor]
        public UserSettingSyncController(
            Lazy<ILoginUserManageService> loginUserManageService)
        {
            
            this.loginUserManageService = loginUserManageService;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        public bool UpdateUserSetting(UserSettingUpdateReq updateReq, out IInterfaceInteractResponse resp)
        {
            resp = null;

            UserSettingHolderItem settingItem = TryGetAndHolderUserSettingItem(new UserSettingItemKey(updateReq.SettingKey, updateReq.LandingInfo.Token));

            // 和上次的设置值一样，不必同步
            if (settingItem.LastSettingContent == updateReq.SettingContent)
            {
                AppLog.Debug($"Content is same with last time, No need update. updateReq:{updateReq.ToString()}");
                return false;
            }

            TryUpdateUserSetting(updateReq, settingItem.LastSettingVersion, out int? newVersion, out resp);
            lock (userSettingItemsLock)
            {
                if (newVersion != null)
                {
                    var originVersion = settingItem.LastSettingVersion;
                    if ((originVersion != null && newVersion >= originVersion) || originVersion == null)
                    {
                        settingItem.LastSettingVersion = newVersion;
                        settingItem.LastSettingContent = updateReq.SettingContent;
                    }
                }
            }

            return newVersion != null;
        }
        
        public string LoadUserSetting(UserSettingLoadReq loadReq, out IInterfaceInteractResponse resp)
        {
            resp = null;

            UserSettingHolderItem settingItem = TryGetAndHolderUserSettingItem(new UserSettingItemKey(loadReq.SettingKey, loadReq.LandingInfo.Token));

            var userSetting = GetUserSetting(loadReq, 2, out resp);
            lock (userSettingItemsLock)
            {
                if (userSetting != null)
                {
                    var newVersion = userSetting.Version;
                    var originVersion = settingItem.LastSettingVersion;
                    if ((originVersion != null && newVersion >= originVersion) || originVersion == null)
                    {
                        settingItem.LastSettingVersion = userSetting.Version;
                        settingItem.LastSettingContent = userSetting.Content;
                    }
                }
            }
            return userSetting?.Content;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            var lastLoginToken = lastLoginResp?.HostingSession?.Token;
            if (lastLoginToken == null) return;

            lock (userSettingItemsLock)
            {
                var tarKeys = userSettingItems.Keys.Where(i => i.UserToken == lastLoginToken).ToArray();
                foreach (var key in tarKeys)
                {
                    userSettingItems.Remove(key);
                }
            }
        }

        private UserSettingHolderItem TryGetAndHolderUserSettingItem(UserSettingItemKey itemKey)
        {
            UserSettingHolderItem settingItem = null;
            lock (userSettingItemsLock)
            {
                if (!userSettingItems.TryGetValue(itemKey, out settingItem))
                {
                    settingItem = new UserSettingHolderItem(itemKey);
                    userSettingItems[itemKey] = settingItem;
                }
            }
            return settingItem;
        }

        private void TryUpdateUserSetting(UserSettingUpdateReq updateReq, 
            int? lastSettingVersion,
            out int? afterSettingVersion,
            out IInterfaceInteractResponse resp)
        {
            afterSettingVersion = null;
            resp = null;

            if (lastSettingVersion == null)
            {
                lastSettingVersion = GetUserSettingVersion(updateReq.LandingInfo, updateReq.SettingKey, 2, out resp);
            }
            if (lastSettingVersion == null) return;

            var syncSetting = new HostingUserSetting
            {
                Content = updateReq.SettingContent,
                Version = (lastSettingVersion.Value + 1)
            };
            var __resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.updateUserSetting(updateReq.LandingInfo, updateReq.SettingKey, syncSetting);
            resp = __resp;
            if (resp.SourceException == null)
            {
                AppLog.Info($"TrySyncUserSetting success, updateReq:{updateReq.ToString()}");
                afterSettingVersion = syncSetting.Version;
                return;
            }

            var businessErrCode = __resp.CustomParsedExceptionResult?.BusinessErrorCode;
            if (ErrorCodeConstants.ERROR_CONFIG_VERSION_LOW == businessErrCode)
            {
                // retry again
                var lastVersion = GetUserSettingVersion(updateReq.LandingInfo, updateReq.SettingKey, 0, out resp);
                if (lastVersion != null)
                {
                    syncSetting.Version = (lastVersion.Value + 1);
                    __resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.updateUserSetting(updateReq.LandingInfo, updateReq.SettingKey, syncSetting);
                    resp = __resp;
                    if (resp.SourceException == null)
                    {
                        AppLog.Info($"TrySyncUserSetting success, updateReq:{updateReq.ToString()}");
                        afterSettingVersion = syncSetting.Version;
                        return;
                    }
                }
            }

            AppLog.Error($"TrySyncUserSetting failed. updateReq:{updateReq.ToString()}");
        }

        private int? GetUserSettingVersion(LandingInfo landingInfo, string userSettingKey, uint retryNumWhenFailed, 
            out IInterfaceInteractResponse resp)
        {
            resp = null;
            var tryNum = retryNumWhenFailed + 1;
            for (var i = 0; i < tryNum; i++)
            {
                var __resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getUserSettingVersion(landingInfo, userSettingKey);
                resp = __resp;
                if (resp.SourceException == null)
                {
                    return __resp.CorrectResult;
                }
            }
            AppLog.Error($"GetUserSettingVersion failed. key:{userSettingKey}, landingInfo:{landingInfo}");
            return null;
        }

        private HostingUserSetting GetUserSetting(UserSettingLoadReq loadReq, uint retryNumWhenFailed,
            out IInterfaceInteractResponse resp)
        {
            resp = null;

            var tryNum = retryNumWhenFailed + 1;
            for (var i = 0; i < tryNum; i++)
            {
                var __resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getUserSetting(loadReq.LandingInfo, loadReq.SettingKey);
                resp = __resp;
                if (resp.SourceException == null)
                {
                    AppLog.Info($"GetUserSetting success, loadReq:{loadReq.ToString()}");
                    return __resp.CorrectResult;
                }
            }
            AppLog.Error($"GetUserSetting failed. loadReq:{loadReq.ToString()}");
            return null;
        }
    }
}
