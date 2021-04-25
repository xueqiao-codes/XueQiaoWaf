using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.terminal.ao;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IUserSettingSyncController
    {
        /// <summary>
        /// 更新用户设置（同步方法）
        /// </summary>
        /// <param name="updateReq">请求</param>
        /// <param name="exception">产生的异常</param>
        /// <returns>是否成功</returns>
        bool UpdateUserSetting(UserSettingUpdateReq updateReq, out IInterfaceInteractResponse resp);

        /// <summary>
        /// 获取用户设置（同步方法）
        /// </summary>
        /// <param name="loadReq">请求</param>
        /// <param name="exception">产生的异常</param>
        /// <returns>用户设置数据</returns>
        string LoadUserSetting(UserSettingLoadReq loadReq, out IInterfaceInteractResponse resp);
    }

    public class UserSettingUpdateReq
    {
        public UserSettingUpdateReq(string settingKey, string settingContent, LandingInfo landingInfo)
        {
            if (string.IsNullOrEmpty(settingKey)) throw new ArgumentException("settingKey can't be empty");
            if (landingInfo == null) throw new ArgumentNullException("landingInfo");
            this.SettingKey = settingKey;
            this.SettingContent = settingContent;
            this.LandingInfo = landingInfo;
        }

        public readonly string SettingKey;
        public readonly string SettingContent;
        public readonly LandingInfo LandingInfo;

        public override string ToString()
        {
            return $"{{Key:{SettingKey}, UserId:{LandingInfo.SubUserId}}}";
        }
    }

    public class UserSettingLoadReq
    {
        public UserSettingLoadReq(string settingKey, LandingInfo landingInfo)
        {
            if (string.IsNullOrEmpty(settingKey)) throw new ArgumentException("settingKey can't be empty");
            if (landingInfo == null) throw new ArgumentNullException("landingInfo");
            this.SettingKey = settingKey;
            this.LandingInfo = landingInfo;
        }

        public readonly string SettingKey;
        public readonly LandingInfo LandingInfo;

        public override string ToString()
        {
            return $"{{Key:{SettingKey}, UserId:{LandingInfo.SubUserId}}}";
        }
    }
}
