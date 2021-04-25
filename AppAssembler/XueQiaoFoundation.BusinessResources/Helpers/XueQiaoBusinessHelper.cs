using lib.xqclient_base.logger;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    public static class XueQiaoBusinessHelper
    {
        /// <summary>
        /// 创建应用的用户数据本地目录
        /// </summary>
        /// <param name="machineId">托管机器 id</param>
        /// <param name="subUserId">用户 id</param>
        /// <param name="environment">登录环境字符串</param>
        /// <returns></returns>
        public static string CreateApplicationUserDataLocalDirectory(long machineId, int subUserId, string environment)
        {
            var userPathItems = new List<string>();
            userPathItems.Add($"Machine_{machineId}");
            userPathItems.Add($"SubUser_{subUserId}");
            userPathItems.Add($"Env_{(environment ?? "")}");

            var userPath = System.Web.HttpUtility.UrlEncode(string.Join("|", userPathItems));
            var tarDir = Path.Combine(XueQiaoConstants.AppLocalDataDirectoryFullName, "User", userPath);
            if (DirectoryHelper.CreateDirectoryIfNeed(tarDir, out Exception ignore) != null)
                return tarDir;
            return null;
        }

        /// <summary>
        /// 获取商品简称
        /// </summary>
        /// <param name="commodity"></param>
        /// <param name="dataLang"></param>
        /// <returns></returns>
        public static string GetCommodityAcronymWithLang(NativeCommodity commodity, XqAppLanguages dataLang)
        {
            string commodityAcronym = null;
            switch (dataLang)
            {
                case XqAppLanguages.ENG:
                    commodityAcronym = commodity?.EngAcronym;
                    break;
                case XqAppLanguages.CN:
                    commodityAcronym = commodity?.CnAcronym;
                    break;
                case XqAppLanguages.TC:
                    commodityAcronym = commodity?.TcAcronym;
                    break;
            }
            return commodityAcronym;
        }

        /// <summary>
        /// 生成交易所的排序列表
        /// </summary>
        /// <param name="srcExchangeList">源交易所列表</param>
        /// <param name="preferredExchangeMicList">交易所 mic 的优先级列表</param>
        /// <param name="preferredExchangeCountryAcronymList">交易所的所在国家缩写优先级列表</param>
        /// <param name="isInnerGetter">某交易所是否为国内交易所的判断方法</param>
        /// <param name="innerExchangeSortedList">获取的国内交易所排序列表</param>
        /// <param name="outterExchangeSortedList">获取的国外交易所排序列表</param>
        public static void GenerateSortedExchangeList(
            IEnumerable<NativeExchange> srcExchanges,
            IEnumerable<string> preferredExchangeMics,
            IEnumerable<string> preferredExchangeCountryAcronyms,
            Func<NativeExchange, bool> isInnerGetter,
            out IEnumerable<NativeExchange> innerExchangeSortedList,
            out IEnumerable<NativeExchange> outterExchangeSortedList)
        {
            innerExchangeSortedList = null;
            outterExchangeSortedList = null;

            Debug.Assert(isInnerGetter != null);

            if (srcExchanges == null) return;
            var innerExchanges = srcExchanges.Where(i => isInnerGetter(i)).ToArray();
            var outterExchanges = srcExchanges.Except(innerExchanges).ToArray();

            innerExchangeSortedList = GetSortedExchangeList(innerExchanges, preferredExchangeMics, preferredExchangeCountryAcronyms);
            outterExchangeSortedList = GetSortedExchangeList(outterExchanges, preferredExchangeMics, preferredExchangeCountryAcronyms);
        }

        private static IEnumerable<NativeExchange> GetSortedExchangeList(
            IEnumerable<NativeExchange> srcExchanges,
            IEnumerable<string> preferredExchangeMics,
            IEnumerable<string> preferredExchangeCountryAcronyms)
        {
            if (srcExchanges == null) return null;

            // 根据交易所所在国家优先级对交易所排序
            var prefExchangeCountryCodeList = preferredExchangeCountryAcronyms?.ToList() ?? new List<string>();
            var exchangesInPrefCountrys = srcExchanges.Where(i => prefExchangeCountryCodeList.Contains(i.CountryCode)).ToArray();
            var exchangesNotInPrefCountrys = srcExchanges.Except(exchangesInPrefCountrys).ToArray();
            var exchangesInPrefCountrysSorted = exchangesInPrefCountrys.GroupBy(i => i.CountryCode).OrderBy(i => prefExchangeCountryCodeList.IndexOf(i.Key)).ToArray();

            var resultList = new List<NativeExchange>();
            // 添加在优先国家列表中的交易所
            foreach (var sortedItem in exchangesInPrefCountrysSorted)
            {
                var _tmp1 = GetSortedExchangeList(sortedItem, preferredExchangeMics: preferredExchangeMics);
                resultList.AddRange(_tmp1);
            }

            // 添加不在优先国家列表中的交易所
            var _tmp2 = GetSortedExchangeList(exchangesNotInPrefCountrys, preferredExchangeMics: preferredExchangeMics);
            resultList.AddRange(_tmp2);
            return resultList;
        }

        private static IEnumerable<NativeExchange> GetSortedExchangeList(
            IEnumerable<NativeExchange> srcExchanges,
            IEnumerable<string> preferredExchangeMics)
        {
            if (srcExchanges == null) return null;

            // 根据交易所优先级列表排序
            var preferredExchangeMicList = preferredExchangeMics?.ToList() ?? new List<string>();
            var exchangesPreferred = srcExchanges.Where(i => preferredExchangeMicList.Contains(i.ExchangeMic)).ToArray();
            var otherExchanges = srcExchanges.Except(exchangesPreferred);

            exchangesPreferred = exchangesPreferred.OrderBy(i => preferredExchangeMicList.IndexOf(i.ExchangeMic)).ToArray();
            return exchangesPreferred.Union(otherExchanges);
        }

        /// <summary>
        /// 格式化合约名称
        /// </summary>
        /// <param name="formartType">格式化类型</param>
        /// <param name="dataLang">数据语言</param>
        /// <param name="commodity">商品信息</param>
        /// <param name="contract">合约信息</param>
        /// <returns></returns>
        public static string FormatContractName(XqContractNameFormatType formartType,
            XqAppLanguages dataLang,
            Tuple<NativeCommodity, NativeContract> contractDetailTuple)
        {
            var commodity = contractDetailTuple?.Item1;
            var contract = contractDetailTuple?.Item2;
            if (commodity == null || contract == null) return null;

            string commodityAcronym = GetCommodityAcronymWithLang(commodity, dataLang);
            string commodityCode = contractDetailTuple?.Item1?.SledCommodityCode;
            string contractCode = contractDetailTuple?.Item2?.SledContractCode;

            return FormatContractNameWithComponents(formartType, commodityAcronym, commodityCode, contractCode);
        }

        /// <summary>
        /// 格式化具有相同根合约的组合合约名称（如跨期组合合约）
        /// </summary>
        /// <param name="formartType"></param>
        /// <param name="dataLang"></param>
        /// <param name="rootArbitrageContractDetailTuple"></param>
        /// <param name="relatedContractDetailTuples"></param>
        /// <returns></returns>
        public static string FormatArbitrageContractNameOfSameRootContract(XqContractNameFormatType formartType,
            XqAppLanguages dataLang,
            Tuple<NativeCommodity, NativeContract> rootArbitrageContractDetailTuple,
            IEnumerable<Tuple<NativeCommodity, NativeContract>> relatedContractDetailTuples)
        {
            var rootArbitrageCommodity = rootArbitrageContractDetailTuple.Item1;
            var rootArbitrageContract = rootArbitrageContractDetailTuple.Item2;
            if (rootArbitrageCommodity == null || rootArbitrageContract == null) return null;

            var relatedContractCodesJoinStr = string.Join(ArbitrageContractRelatedContractsJoinSeperator,
                relatedContractDetailTuples?.Select(i => i.Item2?.SledContractCode ?? ArbitrageContractNameComponentNullDefaultValue)
                .ToArray());
            switch (formartType)
            {
                case XqContractNameFormatType.CommodityAcronym_Code_ContractCode:
                    {
                        var rootCommodityCode = rootArbitrageCommodity.SledCommodityCode;
                        var rootCommodityAcronym = GetCommodityAcronymWithLang(rootArbitrageCommodity, dataLang);
                        return FormatContractNameWithComponents(formartType, rootCommodityAcronym, rootCommodityCode, relatedContractCodesJoinStr);
                    }
                case XqContractNameFormatType.CommodityAcronym_ContractCode:
                    {
                        var rootCommodityAcronym = GetCommodityAcronymWithLang(rootArbitrageCommodity, dataLang);
                        return FormatContractNameWithComponents(formartType, rootCommodityAcronym, null, relatedContractCodesJoinStr);
                    }
                case XqContractNameFormatType.CommodityCode_ContractCode:
                    {
                        var rootCommodityCode = rootArbitrageCommodity.SledCommodityCode;
                        return FormatContractNameWithComponents(formartType, null, rootCommodityCode, relatedContractCodesJoinStr);
                    }
            }
            return null;
        }

        /// <summary>
        /// 格式化具有不同根合约的组合合约名称（如跨品种组合合约）
        /// </summary>
        /// <param name="formartType"></param>
        /// <param name="dataLang"></param>
        /// <param name="relatedContractDetailTuples"></param>
        /// <returns></returns>
        public static string FormatArbitrageContractNameOfDiffRootContract(XqContractNameFormatType formartType,
            XqAppLanguages dataLang,
            IEnumerable<Tuple<NativeCommodity, NativeContract>> relatedContractDetailTuples)
        {
            return string.Join(ArbitrageContractRelatedContractsJoinSeperator,
                relatedContractDetailTuples?.Select(i => FormatContractName(formartType, dataLang, i) ?? ArbitrageContractNameComponentNullDefaultValue)
                .ToArray());
        }

        /// <summary>
        /// 使用各部件字符串格式化合约名称
        /// </summary>
        /// <param name="formartType">格式化类型</param>
        /// <param name="commodityAcronymComponent">商品简称部件</param>
        /// <param name="commodityCodeComponent"商品code部件></param>
        /// <param name="contractCodeComponent">合约code部件</param>
        /// <returns></returns>
        private static string FormatContractNameWithComponents(XqContractNameFormatType formartType,
            string commodityAcronymComponent, string commodityCodeComponent, string contractCodeComponent)
        {
            if (commodityAcronymComponent == null) commodityAcronymComponent = "";
            if (commodityCodeComponent == null) commodityCodeComponent = "";
            if (contractCodeComponent == null) contractCodeComponent = "";

            switch (formartType)
            {
                case XqContractNameFormatType.CommodityAcronym_Code_ContractCode:
                    return $"{commodityAcronymComponent} {commodityCodeComponent}{contractCodeComponent}";
                case XqContractNameFormatType.CommodityAcronym_ContractCode:
                    return $"{commodityAcronymComponent} {contractCodeComponent}";
                case XqContractNameFormatType.CommodityCode_ContractCode:
                    return $"{commodityCodeComponent}{contractCodeComponent}";
            }
            return null;
        }

        private const string ArbitrageContractRelatedContractsJoinSeperator = "&";
        private const string ArbitrageContractNameComponentNullDefaultValue = "--";

        /// <summary>
        /// 创建雪橇订单id
        /// </summary>
        /// <param name="loginMachineId"></param>
        /// <param name="subAccountId"></param>
        /// <param name="loginSubUserId"></param>
        /// <param name="hostingTimestamp"></param>
        /// <param name="localIncreasedId"></param>
        /// <returns></returns>
        public static string CreateXQOrderId(long loginMachineId, long subAccountId,
            int loginSubUserId, long hostingTimestamp, long localIncreasedId)
        {
            return $"{loginMachineId}_{subAccountId}_{loginSubUserId}_{hostingTimestamp}_{localIncreasedId}";
        }

        /// <summary>
        /// 加密登录密码
        /// </summary>
        /// <param name="originPwd"></param>
        /// <returns></returns>
        public static string EncryptLoginPwd(string originPwd)
        {
            if (originPwd == null) return null;
            var bytes = XueQiaoFoundation.Shared.Helper.RSA.RSAEncrypt(XueQiaoConstants.LoginPWDRSAPublicKeyXml, Encoding.UTF8.GetBytes(originPwd));
            return Convert.ToBase64String(bytes);
        }


        /// <summary>
        /// 精确化雪橇合约的相关价格
        /// </summary>
        /// <param name="originPrice">原价格</param>
        /// <param name="srcPriceTick">原价格的 tick </param>
        /// <returns></returns>
        public static double MakePreciseXQContractRelatedPrice(double originPrice, double? srcPriceTick)
        {
            var decimalCount = MathHelper.GetDecimalCount(new decimal(originPrice));
            if (decimalCount <= XueQiaoConstants.XQContractPriceMinimumPirceTick_DecimalCount)
            {
                return originPrice;
            }

            var pricePreciseTick = srcPriceTick ?? XueQiaoConstants.XQContractPriceMinimumPirceTick;
            return MathHelper.MakeValuePrecise(originPrice, pricePreciseTick);
        }

        /// <summary>
        /// 生成雪橇组合默认的名称
        /// </summary>
        /// <param name="legNames">各腿的名称</param>
        /// <returns></returns>
        public static string GenerateXQComposeDefaultName(string[] legNames)
        {
            if (legNames == null) return null;
            return string.Join(" & ", legNames);
        }

        /// <summary>
        /// 获取应用安装包的存放路径。如果相关目录不存在，则会创建
        /// </summary>
        /// <returns></returns>
        public static string GetAppInstallPackageSavePath()
        {
            string fileSaveInDir = null;
            DirectoryInfo downloadsDir = new DirectoryInfo(KnownFolders.Downloads.Path);
            if (downloadsDir.Exists)
            {
                fileSaveInDir = KnownFolders.Downloads.Path;
            }
            else
            {
                fileSaveInDir = Path.Combine(XueQiaoConstants.AppLocalDataDirectoryFullName, "setup_packages");
                if (DirectoryHelper.CreateDirectoryIfNeed(fileSaveInDir, out Exception ignore) == null)
                {
                    fileSaveInDir = null;
                }
            }
            if (fileSaveInDir != null)
            {
                var tarFileName = $"{XueQiaoConstants.AppEnglishName}_setup.exe";
                var fullDownloadFilePath = Path.Combine(fileSaveInDir, tarFileName);
                return fullDownloadFilePath;
            }
            return null;
        }

        /// <summary>
        /// 获取某个音频文件的默认文件全名（全路径）
        /// </summary>
        /// <param name="nameOfFile"></param>
        /// <returns></returns>
        public static string GetDefaultSoundFileFullName(string nameOfFile)
        {
            return Path.Combine(PathHelper.AppSetupDirectoryPath,
                XueQiaoConstants.PUBLISH_RESOURCE_ROOT_DIR_NAME, XueQiaoConstants.PUBLISH_RESOUCE_CHILD_DIR_NAME_SOUNDS,
                nameOfFile);
        }

        /// <summary>
        /// 将 <see cref="XqAppLanguages"/> 类型的语言环境转换为 ThriftApi 的语言
        /// </summary>
        /// <param name="appLanguage"></param>
        /// <returns></returns>
        public static lib.xqclient_base.thriftapi_mediation.Language Convert2ThriftApiLanguage(XqAppLanguages appLanguage)
        {
            if (appLanguage == XqAppLanguages.CN || appLanguage == XqAppLanguages.TC)
            {
                return lib.xqclient_base.thriftapi_mediation.Language.CN;
            }
            return lib.xqclient_base.thriftapi_mediation.Language.EN;
        }

        /// <summary>
        /// 获取应用注册表某个键的值
        /// </summary>
        /// <returns></returns>
        public static object GetApplicationRegistryKey(string keyName)
        {
            object r = null;
            try
            {
                using (var regKey = Registry.CurrentUser.OpenSubKey(XueQiaoConstants.ApplicationRegistryKeyBasePath))
                {
                    if (regKey != null)
                    {
                        r = regKey.GetValue(keyName);
                    }
                }
            }
            catch (Exception e)
            {
                AppLog.Error("Failed to GetApplicationRegistryKey.", e);
            }
            return r;
        }

        /// <summary>
        /// 设置应用注册表某个键的值
        /// </summary>
        /// <param name="keyName">键</param>
        /// <param name="value">值</param>
        public static void SetApplicationRegistryKey(string keyName, object value)
        {
            try
            {
                using (var regKey = Registry.CurrentUser.CreateSubKey(XueQiaoConstants.ApplicationRegistryKeyBasePath))
                {
                    if (regKey != null)
                    {
                        regKey.SetValue(keyName, value);
                    }
                }
            }
            catch (Exception e)
            {
                AppLog.Error("Failed to SetApplicationRegistryKey.", e);
            }
        }

        public static string TrimWhiteSpaceAndRemoveNewLine(string srcStr)
        {
            return srcStr?.Trim() // trim
                .Replace(Environment.NewLine, ""); // remove new line
        }

        /// <summary>
        /// 根据小数位数计算雪橇标的的价格 tick。如果不存在则是用一个默认的
        /// </summary>
        /// <param name="precisionNumber">小数位数</param>
        /// <returns></returns>
        public static double CalculteXqTargetPriceTick(short? precisionNumber, double defaultTick = XueQiaoConstants.XQContractPriceMinimumPirceTick)
        {
            double priceTick = defaultTick;
            if (precisionNumber != null)
            {
                var pn = Math.Abs(precisionNumber.Value);
                if (pn != 0)
                {
                    var sb = new StringBuilder("0.");
                    sb.Append(new string(Enumerable.Repeat('0', pn - 1).ToArray()));
                    sb.Append("1");
                    if (decimal.TryParse(sb.ToString(), out decimal deci))
                    {
                        priceTick = decimal.ToDouble(deci);
                    }
                }
            }
            return priceTick;
        }

        /// <summary>
        /// 根据有效期类型获取有效结束时间
        /// </summary>
        /// <param name="effectDate"></param>
        /// <returns></returns>
        public static long? GetEffectDateEndTimestampMsDependOnType(this HostingXQEffectDate effectDate)
        {
            if (effectDate == null) return null;
            if (effectDate.Type == HostingXQEffectDateType.XQ_EFFECT_PERIOD)
                return effectDate.EndEffectTimestampS * 1000;
            return null;
        }

        public static string FF_K()
        {
            return "eHVlcWlhbzAxMjM0NTY3OHh1ZXFpYW8wMTIzNDU2Nzg=";
        }

        public static string FF_IV()
        {
            return "eHVlcWlhbzAxMjM0NTY3OA==";
        }

        public static string FF_DP()
        {
            return Encoding.UTF8.GetString(AES.Decrypt(Convert.FromBase64String("F4dCwIUw+A3e7ucEcGHC/A=="), Convert.FromBase64String(FF_K()), Convert.FromBase64String(FF_IV())))
                + Encoding.UTF8.GetString(AES.Decrypt(Convert.FromBase64String("Q3tAR5H2ebizMmzHAu4T2w=="), Convert.FromBase64String(FF_K()), Convert.FromBase64String(FF_IV())));
        }
    }
}
