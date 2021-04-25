using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    public static class XqTargetDMHelper
    {
        /// <summary>
        /// 根据类型 <see cref="IXqTargetDM"/> 设置的数据获取合适的标的名称
        /// </summary>
        /// <param name="xqTargetDM"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetAppropriateTargetName(IXqTargetDM xqTargetDM, XqAppLanguages language)
        {
            if (xqTargetDM == null) return null;

            var GetContractName = new Func<TargetContract_TargetContractDetail, XqAppLanguages, string>((_c, _l) =>
            {
                if (_c == null) return null;
                string contractName = null;
                switch (_l)
                {
                    case XqAppLanguages.ENG:
                        contractName = _c.EngDisplayName;
                        break;
                    case XqAppLanguages.CN:
                        contractName = _c.CnDisplayName;
                        break;
                    case XqAppLanguages.TC:
                        contractName = _c.TcDisplayName;
                        break;
                    default:
                        contractName = _c.CnDisplayName;
                        break;
                }
                return contractName;
            });

            string xqTargetName = null;
            if (xqTargetDM.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                xqTargetName = GetContractName(xqTargetDM.TargetContractDetailContainer, language);
            }
            else if (xqTargetDM.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var composeViewContainer = xqTargetDM.TargetComposeUserComposeViewContainer;
                var composeDetailContainer = xqTargetDM.TargetComposeDetailContainer;
                if (composeViewContainer?.UserComposeView != null)
                {
                    xqTargetName = composeViewContainer?.UserComposeView?.AliasName;
                }
                else if (composeDetailContainer != null)
                {
                    string[] legNames = composeDetailContainer.DetailLegs?.ToArray()
                            .Select(i => GetContractName(i.LegDetailContainer, language) ?? "")
                            .Where(i => !string.IsNullOrEmpty(i)).ToArray() ?? new string[] { };
                    xqTargetName = XueQiaoBusinessHelper.GenerateXQComposeDefaultName(legNames);
                }
            }

            return xqTargetName;
        }

        /// <summary>
        /// 使用合适的名称更新<see cref="IXqTargetDM"/>类型的标的名称
        /// </summary>
        /// <param name="xqTargetDM"></param>
        /// <param name="language"></param>
        public static void InvalidateTargetNameWithAppropriate(this IXqTargetDM xqTargetDM, XqAppLanguages language)
        {
            if (xqTargetDM == null) return;
            xqTargetDM.TargetName = GetAppropriateTargetName(xqTargetDM, language);
        }
    }
}
