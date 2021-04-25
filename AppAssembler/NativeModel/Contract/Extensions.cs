using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.contract.standard;

namespace NativeModel.Contract
{
    public static class Extensions
    {
        public static NativeCommodity ToNativeCommodity(this SledCommodity sledCommodity)
        {
            var dest = new NativeCommodity();

            dest.SledCommodityId = sledCommodity.SledCommodityId;
            dest.ExchangeMic = sledCommodity.ExchangeMic;
            dest.SledCommodityType = sledCommodity.SledCommodityType.GetHashCode();
            dest.SledCommodityCode = sledCommodity.SledCommodityCode;

            if (sledCommodity.RelateCommodityIds != null)
                dest.RelateCommodityIds = new ObservableCollection<int>(sledCommodity.RelateCommodityIds);

            dest.TradeCurrency = sledCommodity.TradeCurrency;
            dest.ZoneId = sledCommodity.ZoneId;
            dest.ContractSize = sledCommodity.ContractSize;
            dest.TickSize = sledCommodity.TickSize;
            dest.Denominator = sledCommodity.Denominator;
            dest.FillNativeCommodityNameInfos(sledCommodity);
            dest.ActiveStartTimestamp = sledCommodity.ActiveStartTimestamp;
            dest.ActiveEndTimestamp = sledCommodity.ActiveEndTimestamp;
            dest.CreateTimestamp = sledCommodity.CreateTimestamp;
            dest.LastModityTimestamp = sledCommodity.LastModityTimestamp;

            if(sledCommodity.SledCommodityConfig != null)
                dest.Configs = new ObservableCollection<SledCommodityConfig>(sledCommodity.SledCommodityConfig);
            
            return dest;
        }

        public static void FillNativeCommodityNameInfos(this NativeCommodity srcCommodity, NativeCommodity filler)
        {
            if (srcCommodity == null || filler == null) return;
            srcCommodity.EngAcronym = filler.EngAcronym;
            srcCommodity.CnAcronym = filler.CnAcronym;
            srcCommodity.TcAcronym = filler.TcAcronym;
            srcCommodity.EngName = filler.EngName;
            srcCommodity.CnName = filler.CnName;
            srcCommodity.TcName = filler.TcName;
        }

        public static void FillNativeCommodityNameInfos(this NativeCommodity srcCommodity, SledCommodity filler)
        {
            if (srcCommodity == null || filler == null) return;
            srcCommodity.EngAcronym = filler.EngAcronym;
            srcCommodity.CnAcronym = filler.CnAcronym;
            srcCommodity.TcAcronym = filler.TcAcronym;
            srcCommodity.EngName = filler.EngName;
            srcCommodity.CnName = filler.CnName;
            srcCommodity.TcName = filler.TcName;
        }

        public static NativeContract ToNativeContract(this SledContract sledContract)
        {
            var dest = new NativeContract();

            dest.SledContractId = sledContract.SledContractId;
            dest.SledCommodityId = sledContract.SledCommodityId;
            dest.SledContractCode = sledContract.SledContractCode;
            
            if (sledContract.RelateContractIds != null)
                dest.RelateContractIds = new ObservableCollection<int>(sledContract.RelateContractIds);

            dest.SledTag = sledContract.SledTag;
            dest.ContractEngName = sledContract.ContractEngName;
            dest.ContractCnName = sledContract.ContractCnName;
            dest.ContractTcName = sledContract.ContractTcName;
            dest.ContractExpDate = sledContract.ContractExpDate;
            dest.LastTradeDate = sledContract.LastTradeDate;
            dest.FirstNoticeDate = sledContract.FirstNoticeDate;
            dest.SubscribeXQQuote = sledContract.SubscribeXQQuote;
            dest.ActiveStartTimestamp = sledContract.ActiveStartTimestamp;
            dest.ActiveEndTimestamp = sledContract.ActiveEndTimestamp;
            dest.CreateTimestamp = sledContract.CreateTimestamp;
            dest.LastModityTimestamp = sledContract.LastModityTimestamp;
            dest.IsDisabled = (sledContract.ContractStatus == ContractStatus.DISABLED);
            dest.IsExpired = (sledContract.ContractStatus == ContractStatus.EXPIRED);
            
            return dest;
        }

        public static NativeExchange ToNativeExchange(this SledExchange sledExchange)
        {
            var dest = new NativeExchange();

            dest.SledExchangeId = sledExchange.SledExchangeId;
            dest.ExchangeMic = sledExchange.ExchangeMic;
            dest.Country = sledExchange.Country;
            dest.CountryCode = sledExchange.CountryCode;
            dest.OperatingMic = sledExchange.OperatingMic;
            dest.OperatingMicType = Enum.GetName(typeof(ExchangeOperatingMicType), sledExchange.OperatingMicType);
            dest.NameInstitution = sledExchange.NameInstitution;
            dest.Acronym = sledExchange.Acronym;
            dest.City = sledExchange.City;
            dest.Website = sledExchange.Website;
            dest.CnName = sledExchange.CnName;
            dest.CnAcronym = sledExchange.CnAcronym;
            dest.ActiveStartTimestamp = sledExchange.ActiveStartTimestamp;
            dest.ActiveEndTimestamp = sledExchange.ActiveEndTimestamp;
            dest.CreateTimestamp = sledExchange.CreateTimestamp;
            dest.LastModityTimestamp = sledExchange.LastModityTimestamp;

            return dest;
        }
    }
}
