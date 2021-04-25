using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Waf.Applications;
using TouyanAssembler.app.viewmodel;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace TouyanAssembler.app.controller
{
    internal delegate void LoginDialogDidLogined(LoginRegDialogCtrl ctrl, XiaohaChartLandingInfo loginLandingInfo);

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class LoginRegDialogCtrl : IController
    {
        private readonly LoginVM contentVM;
        private readonly IMessageWindowService messageWindowService;

        private readonly DelegateCommand submitCmd;
        private readonly DelegateCommand cancelCmd;
        private readonly DelegateCommand reqGetVerifyCodeCmd;

        private readonly TaskFactory submitTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        private CancellationTokenSource submitCLTS;
        private readonly object submitCLTSLock = new object();
        
        private IMessageWindow dialog;
        private bool isSubmiting;
        private bool isRequestingGetVerifyCode;

        private System.Timers.Timer getVerifyCodeEnabledCountDownTimer;
        private int getVerifyCodeEnabledCountDownSeconds;
        private bool isCountingDownGetVerifyCodeEnabled;

        [ImportingConstructor]
        public LoginRegDialogCtrl(LoginVM contentVM,
            IMessageWindowService messageWindowService)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;

            submitCmd = new DelegateCommand(Submit, CanSubmit);
            cancelCmd = new DelegateCommand(LeaveDialog);
            reqGetVerifyCodeCmd = new DelegateCommand(ReqGetVerifyCode, CanReqGetVerifyCode);
        }

        /// <summary>
        /// 初始值是否为注册页面。否则为登录页面
        /// </summary>
        public bool InitialIsRegisterView { get; set; }

        public object DialogOwner { get; set; }

        public Action DialogContentRendered { get; set; }

        public LoginDialogDidLogined DidLoginedHandler { get; set; }

        /// <summary>
        /// 登录或注册结果。True 表示成功，False 失败，null 表示取消。
        /// </summary>
        public bool? LoginOrRegResult { get; private set; }

        public void Initialize()
        {
            contentVM.IsRegisterView = this.InitialIsRegisterView;
            contentVM.SubmitCmd = submitCmd;
            contentVM.CancelCmd = cancelCmd;
            contentVM.ReqGetVerifyCodeCmd = reqGetVerifyCodeCmd;
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropertyChanged, "");
        }

        public void Run()
        {
            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, false,
                "", contentVM.View);
            this.dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            DidLoginedHandler = null;
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropertyChanged, "");
        }

        private void ContentVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginVM.TelNumber))
            {
                submitCmd?.RaiseCanExecuteChanged();
                reqGetVerifyCodeCmd?.RaiseCanExecuteChanged();
            }
            else if (e.PropertyName == nameof(LoginVM.VerifyCode))
            {
                submitCmd?.RaiseCanExecuteChanged();
            }
        }

        private CancellationToken AcquireSubmitCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (submitCLTSLock)
            {
                if (submitCLTS == null)
                {
                    submitCLTS = new CancellationTokenSource();
                }
                clt = submitCLTS.Token;
            }
            return clt;
        }

        private void CancelSubmit()
        {
            lock (submitCLTSLock)
            {
                if (submitCLTS != null)
                {
                    submitCLTS.Cancel();
                    submitCLTS.Dispose();
                    submitCLTS = null;
                }
            }
        }

        public bool CanSubmit()
        {
            if (isSubmiting) return false;
            if (!StringHelper.IsNotNagtive(contentVM.TelNumber)
                || string.IsNullOrEmpty(contentVM.VerifyCode))
                return false;
            return true;
        }

        public void Submit()
        {
            var telNumber = contentVM.TelNumber;
            var verifyCode = contentVM.VerifyCode;
            var isRegister = contentVM.IsRegisterView;
            
            CancelSubmit();
            var clt = AcquireSubmitCLT();
            submitTaskFactory.StartNew(() => 
            {
                if (clt.IsCancellationRequested) return;

                UpdateIsSubmiting(true);

                var httpStub = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub;
                IInterfaceInteractResponse<XiaohaChartLandingInfo> loginOrRegResp = null;
                if (isRegister)
                {
                    // 注册
                    loginOrRegResp = httpStub.signUp(new SignUpReq { Tel = telNumber, VerifyCode = verifyCode });
                }
                else
                {
                    // 登录
                    loginOrRegResp = httpStub.login(new LoginReq { Tel = telNumber, Password = verifyCode });
                }
                
                UpdateIsSubmiting(false);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (clt.IsCancellationRequested) return;

                    var landingInfo = loginOrRegResp?.CorrectResult;
                    var loginOrRegTitle = isRegister ? "注册" : "登录";
                    if (loginOrRegResp == null || loginOrRegResp.SourceException != null || landingInfo == null)
                    {
                        // 失败
                        var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(loginOrRegResp, $"{loginOrRegTitle}失败\n");
                        messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, errMsg, "知道了");
                        return;
                    }

                    // 成功
                    this.LoginOrRegResult = true;
                    
                    DidLoginedHandler?.Invoke(this, landingInfo);
                    InternalCloseDialog();
                });
            });
        }

        private void LeaveDialog()
        {
            CancelSubmit();
            InternalCloseDialog();
        }

        private bool CanReqGetVerifyCode()
        {
            if (!StringHelper.IsNotNagtive(contentVM.TelNumber))
                return false;

            return !isRequestingGetVerifyCode 
                && !isSubmiting
                && !isCountingDownGetVerifyCodeEnabled;
        }

        private void ReqGetVerifyCode()
        {
            if (isRequestingGetVerifyCode) return;

            UpdateIsRequestingGetVerifyCode(true);
            var telNumber = contentVM.TelNumber;
            var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
            XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                .sendVerifyCodeAsync(telNumber, CancellationToken.None, siip)
                .ContinueWith(t => 
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        UpdateIsRequestingGetVerifyCode(false);

                        if (resp == null || resp.SourceException != null)
                        {
                            // 失败
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "请求获取验证码失败\n");
                            messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, errMsg, "知道了");
                            return;
                        }

                        DisposeGetVerifyCodeEnabledCountDownTimer();
                        InitGetVerifyCodeEnabledCountDownTimerIfNeed();
                        UpdateGetVerifyCodeEnabledCountDownSeconds(60);
                        UpdateIsCountingDownGetVerifyCodeEnabled(true);
                        this.getVerifyCodeEnabledCountDownTimer.Start();
                    });
                });
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }

        private void UpdateIsSubmiting(bool value)
        {
            this.isSubmiting = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                contentVM.IsSubmiting = value;
                submitCmd?.RaiseCanExecuteChanged();
                reqGetVerifyCodeCmd?.RaiseCanExecuteChanged();
            });
        }

        private void UpdateIsRequestingGetVerifyCode(bool value)
        {
            this.isRequestingGetVerifyCode = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                reqGetVerifyCodeCmd?.RaiseCanExecuteChanged();
            });
        }

        private void InitGetVerifyCodeEnabledCountDownTimerIfNeed()
        {
            var timer = this.getVerifyCodeEnabledCountDownTimer;
            if (timer == null)
            {
                timer = new System.Timers.Timer(1000);
                timer.Elapsed += GetVerifyCodeEnabledCountDownTimer_Elapsed;
                this.getVerifyCodeEnabledCountDownTimer = timer;
            }
        }
        
        private void DisposeGetVerifyCodeEnabledCountDownTimer()
        {
            if (getVerifyCodeEnabledCountDownTimer != null)
            {
                getVerifyCodeEnabledCountDownTimer.Elapsed -= GetVerifyCodeEnabledCountDownTimer_Elapsed;
                getVerifyCodeEnabledCountDownTimer.Stop();
                getVerifyCodeEnabledCountDownTimer.Dispose();
                getVerifyCodeEnabledCountDownTimer = null;
            }
        }
        
        private void UpdateGetVerifyCodeEnabledCountDownSeconds(int value)
        {
            this.getVerifyCodeEnabledCountDownSeconds = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                contentVM.GetVerifyCodeEnabledCountDownSeconds = value;
            });
        }

        private void UpdateIsCountingDownGetVerifyCodeEnabled(bool value)
        {
            this.isCountingDownGetVerifyCodeEnabled = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                contentVM.IsCountingDownGetVerifyCodeEnabled = value;
                reqGetVerifyCodeCmd?.RaiseCanExecuteChanged();
            });
        }

        private void GetVerifyCodeEnabledCountDownTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!isCountingDownGetVerifyCodeEnabled) return;

            var coundownSecs = getVerifyCodeEnabledCountDownSeconds - 1;
            if (coundownSecs <= 0)
            {
                // stop timer
                var timer = getVerifyCodeEnabledCountDownTimer;
                if (timer != null)
                {
                    timer.Stop();
                }
                UpdateIsCountingDownGetVerifyCodeEnabled(false);
            }
            UpdateGetVerifyCodeEnabledCountDownSeconds(coundownSecs);
        }
    }
}
