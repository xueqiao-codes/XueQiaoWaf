using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Trade
{
    public class NativeComposeLeg : Model
    {
        public NativeComposeLeg(long composeGraphId, long sledContractId)
        {
            this.ComposeGraphId = composeGraphId;
            this.SledContractId = sledContractId;
        }

        public long ComposeGraphId { private set; get; }

        public long SledContractId { private set; get; }

        private string _variableName;
        public string VariableName
        {
            get
            {
                return _variableName;
            }
            set
            {
                SetProperty(ref _variableName, value);
            }
        }

        private int _quantity;
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                SetProperty(ref _quantity, value);
            }
        }

        private ClientTradeDirection _tradeDirection;
        public ClientTradeDirection TradeDirection
        {
            get
            {
                return _tradeDirection;
            }
            set
            {
                SetProperty(ref _tradeDirection, value);
            }
        }

        private string sledContractCode;
        public string SledContractCode
        {
            get { return sledContractCode; }
            set { SetProperty(ref sledContractCode, value); }
        }

        private long sledCommodityId;
        public long SledCommodityId
        {
            get { return sledCommodityId; }
            set { SetProperty(ref sledCommodityId, value); }
        }

        private int sledCommodityType;
        public int SledCommodityType
        {
            get { return sledCommodityType; }
            set { SetProperty(ref sledCommodityType, value); }
        }

        private string sledCommodityCode;
        public string SledCommodityCode
        {
            get { return sledCommodityCode; }
            set { SetProperty(ref sledCommodityCode, value); }
        }

        private string sledExchangeMic;
        public string SledExchangeMic
        {
            get { return sledExchangeMic; }
            set { SetProperty(ref sledExchangeMic, value); }
        }
    }
}
