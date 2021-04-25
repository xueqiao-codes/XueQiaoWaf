using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    public static class XqAppPreference_ModelHelper
    {
        public static XqAppPreference ToXqAppPreference(XqAppPreferenceDM dataModel)
        {
            if (dataModel == null) return null;
            return new XqAppPreference
            {
                Language = dataModel.Language.ToString(),
                AppTheme = dataModel.AppTheme.ToString(),

                OrderErrAudioFileName = dataModel.OrderErrAudioFileName,
                OrderAmbiguousAudioFileName = dataModel.OrderAmbiguousAudioFileName,
                OrderTriggeredAudioFileName = dataModel.OrderTriggeredAudioFileName,
                OrderTradedAudioFileName = dataModel.OrderTradedAudioFileName,
                LameTradedAudioFileName = dataModel.LameTradedAudioFileName,
                OrderOtherNotifyAudioFileName = dataModel.OrderOtherNotifyAudioFileName,

                PlaceOrderNeedConfirm = dataModel.PlaceOrderNeedConfirm,
                SuspendOrderNeedConfirm = dataModel.SuspendOrderNeedConfirm,
                ResumeOrderNeedConfirm = dataModel.ResumeOrderNeedConfirm,
                StrongChaseOrderNeedConfirm = dataModel.StrongChaseOrderNeedConfirm,
                RevokeOrderNeedConfirm = dataModel.RevokeOrderNeedConfirm,
            };
        }

        public static XqAppPreferenceDM ToXqAppPreferenceDM(XqAppPreference normalModel)
        {

            if (normalModel == null) return null;
            
            var dataModel = new XqAppPreferenceDM
            {
                Language = XqAppLanguages.CN,
                AppTheme = XqAppThemeType.LIGHT,

                OrderErrAudioFileName = normalModel.OrderErrAudioFileName,
                OrderAmbiguousAudioFileName = normalModel.OrderAmbiguousAudioFileName,
                OrderTriggeredAudioFileName = normalModel.OrderTriggeredAudioFileName,
                OrderTradedAudioFileName = normalModel.OrderTradedAudioFileName,
                LameTradedAudioFileName = normalModel.LameTradedAudioFileName,
                OrderOtherNotifyAudioFileName = normalModel.OrderOtherNotifyAudioFileName,

                PlaceOrderNeedConfirm = normalModel.PlaceOrderNeedConfirm ?? false,
                SuspendOrderNeedConfirm = normalModel.SuspendOrderNeedConfirm ?? false,
                ResumeOrderNeedConfirm = normalModel.ResumeOrderNeedConfirm ?? false,
                StrongChaseOrderNeedConfirm = normalModel.StrongChaseOrderNeedConfirm ?? false,
                RevokeOrderNeedConfirm = normalModel.RevokeOrderNeedConfirm ?? false,
            };

            var srcLang = normalModel.Language ?? "";
            if (Enum.TryParse(srcLang, out XqAppLanguages _lang0))
            {
                dataModel.Language = _lang0;
            }
            else if (Enum.TryParse(srcLang.ToUpper(), out XqAppLanguages _lang1))
            {
                dataModel.Language = _lang1;
            }

            var srcAppTheme = normalModel.AppTheme ?? "";
            if (Enum.TryParse(srcAppTheme, out XqAppThemeType _themeType0))
            {
                dataModel.AppTheme = _themeType0;
            }
            else if (Enum.TryParse(srcAppTheme.ToUpper(), out XqAppThemeType _themeType1))
            {
                dataModel.AppTheme = _themeType1;
            }

            return dataModel;
        }
    }
}
