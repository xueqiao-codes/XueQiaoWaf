using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using IDLAutoGenerated.Util;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using Manage.Applications.DataModels;
using Manage.Applications.Domain;
using Manage.Applications.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.broker;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccounEditDialogController : IController
    {
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly FundAccounEditDialogContentViewModel pageViewModel;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand okCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow dialog;

        private readonly EditFundAccount editingAccount;

        [ImportingConstructor]
        public FundAccounEditDialogController(ILoginDataService loginDataService,
               Lazy<ILoginUserManageService> loginUserManageService,
            IMessageWindowService messageWindowService,
            FundAccounEditDialogContentViewModel pageViewModel,
            IEventAggregator eventAggregator)
        {
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.messageWindowService = messageWindowService;
            this.pageViewModel = pageViewModel;
            this.eventAggregator = eventAggregator;

            okCmd = new DelegateCommand(DoUpdateTradeAccount);
            cancelCmd = new DelegateCommand(LeaveDialog);

            editingAccount = new EditFundAccount();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public FundAccountModel ToEditAccountItem { get; set; }

        public object DialogOwner { get; set; }

        /// <summary>
        /// 已修改的结果账户
        /// </summary>
        public EditFundAccount UpdatedAccountResult { get; private set; }

        public void Initialize()
        {
            if (ToEditAccountItem == null) throw new ArgumentNullException("ToEditAccountItem");
            if (ToEditAccountItem.AccountMeta == null) throw new ArgumentNullException("ToEditAccountItem.AccountMeta");

            editingAccount.LoginUserName = ToEditAccountItem.AccountMeta?.LoginUserName;
            editingAccount.AccountAlias = ToEditAccountItem.AccountMeta?.TradeAccountRemark;
            editingAccount.Es9_AuthCode = ToEditAccountItem.AccountMeta?.GetEs9_AuthCode();
            
            string es3_AppId = null;
            string es3_CertInfo = null;
            ToEditAccountItem.AccountMeta?.GetEs3Properties(out es3_AppId, out es3_CertInfo);

            editingAccount.Es3_AppId = es3_AppId;
            editingAccount.Es3_CertInfo = es3_CertInfo;

            string ctp_AppId = null;
            string ctp_AuthCode = null;
            ToEditAccountItem.AccountMeta?.GetCTPProperties(out ctp_AppId, out ctp_AuthCode);

            editingAccount.Ctp_AppId = ctp_AppId;
            editingAccount.Ctp_AuthCode = ctp_AuthCode;

            pageViewModel.AccountInfo = ToEditAccountItem;
            pageViewModel.EditAccount = editingAccount;

            pageViewModel.OkCmd = okCmd;
            pageViewModel.CancelCmd = cancelCmd;
        }

        public void Run()
        {
            LoadAllAccessEntries();

            var dialogTitle = "编辑资金账户";
            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, 
                true, dialogTitle, pageViewModel.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }
        
        private IInterfaceInteractResponse<IEnumerable<BrokerAccessEntry>> QueryBrokerAllAccessEntries(int brokerId, CancellationToken ct)
        {
            var queryPageSize = 50;
            IInterfaceInteractResponse<BrokerAccessPage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<BrokerAccessEntry>(pageIndex => {
                if (ct.IsCancellationRequested) return null;

                var serviceStub = XqThriftLibConfigurationManager.SharedInstance.BrokerServiceHttpStub;
                var option = new ReqBrokerAccessOption
                {
                    BrokerId = new List<int> { brokerId }
                };
                // first time
                var resp = serviceStub.reqBrokerAccess(option, pageIndex, queryPageSize);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<BrokerAccessEntry>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page?.ToArray()
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.EntryId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<BrokerAccessEntry>>(firstPageResp.Servant,
                firstPageResp.InterfaceName,
                firstPageResp.SourceException,
                firstPageResp.HasTransportException,
                firstPageResp.HttpResponseStatusCode,
                queriedItems)
            {
                CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                InteractInformation = firstPageResp.InteractInformation
            };

            return tarResp;
        }

        private void LoadAllAccessEntries()
        {
            var tradeBrokerId = ToEditAccountItem.AccountMeta.TradeBrokerId;
            var brokerPlatform = ToEditAccountItem.AccountMeta.BrokerTechPlatform.ToBrokerPlatform();
            Task.Run(() =>
            {
                var resp = QueryBrokerAllAccessEntries(tradeBrokerId, CancellationToken.None);
                if (resp == null) return null;
                if (resp.SourceException != null)
                {
                    // error handle
                    return null;
                }

                // 过滤平台
                var accessEntries = resp.CorrectResult?.Where(i=>i.Platform == brokerPlatform).ToArray();
                return accessEntries;
            })
            .ContinueWith(t=> 
            {
                var accessEntries = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    int? originAccessEntryId = null;
                    BrokerPlatform? originBrokerPlatform = null;
                    if (pageViewModel.SelectedAccessEntry != null)
                    {
                        originAccessEntryId = pageViewModel.SelectedAccessEntry.EntryId;
                        originBrokerPlatform = pageViewModel.SelectedAccessEntry.Platform;
                    }
                    else
                    {
                        originAccessEntryId = ToEditAccountItem.AccountMeta?.TradeBrokerAccessId;
                        originBrokerPlatform = ToEditAccountItem.AccountMeta?.BrokerTechPlatform.ToBrokerPlatform();
                    }

                    BrokerAccessEntry newSelectAccessEntry = null;
                    if (originAccessEntryId != null && originBrokerPlatform != null)
                        newSelectAccessEntry = accessEntries?.FirstOrDefault(i => i.EntryId == originAccessEntryId && i.Platform == originBrokerPlatform);
                    else
                        newSelectAccessEntry = accessEntries?.FirstOrDefault();

                    // update AvailableAccessEntries and newSelectAccessEntry
                    pageViewModel.AvailableAccessEntries.Clear();
                    pageViewModel.AvailableAccessEntries.AddRange(accessEntries);
                    pageViewModel.SelectedAccessEntry = newSelectAccessEntry;
                });
            });
        }

        private void DoUpdateTradeAccount()
        {
            var newAccountProperties = new Dictionary<string, string>();
            if (!editingAccount.Validate())
            {
                var errorMsg = editingAccount.JoinErrors();
                messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(pageViewModel.View), null, null, null,
                    string.IsNullOrEmpty(errorMsg) ? "请输入必填信息" : errorMsg);
                return;
            }

            var selectAccessEntry = pageViewModel.SelectedAccessEntry;
            if (selectAccessEntry == null)
            {
                messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(pageViewModel.View), null, null, null,
                    "请选择一个席位");
                return;
            }

            var toUpdateAccountMeta = pageViewModel.AccountInfo?.AccountMeta;
            if (toUpdateAccountMeta == null) return;

            var brokerPlatform = toUpdateAccountMeta?.BrokerTechPlatform;
            if (brokerPlatform == BrokerTechPlatform.TECH_ESUNNY_9)
            {
                // 易盛9.0，需要填写 authcode 字段
                var es9AuthCode = XueQiaoBusinessHelper.TrimWhiteSpaceAndRemoveNewLine(editingAccount.Es9_AuthCode);
                if (string.IsNullOrWhiteSpace(es9AuthCode))
                {
                    messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(pageViewModel.View), null, null, null, "请填写 Auth code");
                    return;
                }
                else
                {
                    newAccountProperties[trade_hosting_basicConstants.ESUNNY9_AUTHCODE_PROPKEY] = es9AuthCode;
                }
            }
            else if (brokerPlatform == BrokerTechPlatform.TECH_ESUNNY_3)
            {
                // 易盛3.0，需要填写 appid, certinfo 字段
                var appId = XueQiaoBusinessHelper.TrimWhiteSpaceAndRemoveNewLine(editingAccount.Es3_AppId);
                var certInfo = XueQiaoBusinessHelper.TrimWhiteSpaceAndRemoveNewLine(editingAccount.Es3_CertInfo);
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(certInfo))
                {
                    messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(pageViewModel.View), null, null, null, "请填写 app id 和 cert info");
                    return;
                }
                else
                {
                    newAccountProperties[trade_hosting_basicConstants.ESUNNY3_APPID_PROPKEY] = appId;
                    newAccountProperties[trade_hosting_basicConstants.ESUNNY3_CERTINFO_PROPKEY] = certInfo;
                }
            }
            else if (brokerPlatform == BrokerTechPlatform.TECH_CTP)
            {
                // CTP，appid, autocode 字段填写可选
                var appId = XueQiaoBusinessHelper.TrimWhiteSpaceAndRemoveNewLine(editingAccount.Ctp_AppId);
                var authCode = XueQiaoBusinessHelper.TrimWhiteSpaceAndRemoveNewLine(editingAccount.Ctp_AuthCode);
                newAccountProperties[trade_hosting_basicConstants.CTP_STS_APPID] = appId ?? "";
                newAccountProperties[trade_hosting_basicConstants.CTP_STS_AUTHCODE] = authCode ?? "";
            }

            var editPassword = pageViewModel.EditPassword;
            if (!string.IsNullOrEmpty(editPassword))
            {
                if (messageWindowService.ShowQuestionDialog(UIHelper.GetWindowOfUIElement(pageViewModel.View), null, null, null, "确定要同时修改密码吗？", false, "确定修改", "取消")
                    != true)
                {
                    return;
                }
            }

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            var updatingAccount = new HostingTradeAccount
            {
                TradeAccountId = toUpdateAccountMeta.TradeAccountId,
                LoginUserName = editingAccount.LoginUserName,
                TradeAccountRemark = editingAccount.AccountAlias,
                AccountProperties = newAccountProperties,
                TradeBrokerId = selectAccessEntry.BrokerId,
                TradeBrokerAccessId = selectAccessEntry.EntryId,
            };

            if (!string.IsNullOrEmpty(editPassword))
            {
                updatingAccount.LoginPassword = editPassword;
            }

            var interactParams = new StubInterfaceInteractParams { LogInterfaceRequestArgs = false };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                .updateTradeAccountInfoAsync(landingInfo, updatingAccount, CancellationToken.None, interactParams)
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (resp == null || resp.SourceException != null)
                        {
                            var owner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
                            if (owner != null)
                            {
                                var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "修改资金账户失败\n");
                                messageWindowService.ShowMessageDialog(owner, null, null, null, errMsg);
                            }
                            return;
                        }

                        this.UpdatedAccountResult = editingAccount;
                        InternalCloseDialog();
                    });
                });
        }

        private void LeaveDialog()
        {
            UpdatedAccountResult = null;
            InternalCloseDialog();
        }
        
        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }
    }
}
