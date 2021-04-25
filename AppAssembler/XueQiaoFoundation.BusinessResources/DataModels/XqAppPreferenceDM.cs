using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class XqAppPreferenceDM : Model
    {
        private XqAppLanguages language;
        public XqAppLanguages Language
        {
            get { return language; }
            set { SetProperty(ref language, value); }
        }

        private XqAppThemeType appTheme;
        public XqAppThemeType AppTheme
        {
            get { return appTheme; }
            set { SetProperty(ref appTheme, value); }
        }

        private string orderErrAudioFileName;
        public string OrderErrAudioFileName
        {
            get { return orderErrAudioFileName; }
            set { SetProperty(ref orderErrAudioFileName, value); }
        }

        private string orderAmbiguousAudioFileName;
        public string OrderAmbiguousAudioFileName
        {
            get { return orderAmbiguousAudioFileName; }
            set { SetProperty(ref orderAmbiguousAudioFileName, value); }
        }

        private string orderTriggeredAudioFileName;
        public string OrderTriggeredAudioFileName
        {
            get { return orderTriggeredAudioFileName; }
            set { SetProperty(ref orderTriggeredAudioFileName, value); }
        }

        private string orderTradedAudioFileName;
        public string OrderTradedAudioFileName
        {
            get { return orderTradedAudioFileName; }
            set { SetProperty(ref orderTradedAudioFileName, value); }
        }

        private string lameTradedAudioFileName;
        public string LameTradedAudioFileName
        {
            get { return lameTradedAudioFileName; }
            set { SetProperty(ref lameTradedAudioFileName, value); }
        }

        private string orderOtherNotifyAudioFileName;
        public string OrderOtherNotifyAudioFileName
        {
            get { return orderOtherNotifyAudioFileName; }
            set { SetProperty(ref orderOtherNotifyAudioFileName, value); }
        }
        
        private bool placeOrderNeedConfirm;
        public bool PlaceOrderNeedConfirm
        {
            get { return placeOrderNeedConfirm; }
            set { SetProperty(ref placeOrderNeedConfirm, value); }
        }

        private bool suspendOrderNeedConfirm;
        public bool SuspendOrderNeedConfirm
        {
            get { return suspendOrderNeedConfirm; }
            set { SetProperty(ref suspendOrderNeedConfirm, value); }
        }

        private bool resumeOrderNeedConfirm;
        public bool ResumeOrderNeedConfirm
        {
            get { return resumeOrderNeedConfirm; }
            set { SetProperty(ref resumeOrderNeedConfirm, value); }
        }

        private bool strongChaseOrderNeedConfirm;
        public bool StrongChaseOrderNeedConfirm
        {
            get { return strongChaseOrderNeedConfirm; }
            set { SetProperty(ref strongChaseOrderNeedConfirm, value); }
        }

        private bool revokeOrderNeedConfirm;
        public bool RevokeOrderNeedConfirm
        {
            get { return revokeOrderNeedConfirm; }
            set { SetProperty(ref revokeOrderNeedConfirm, value); }
        }
    }
}
