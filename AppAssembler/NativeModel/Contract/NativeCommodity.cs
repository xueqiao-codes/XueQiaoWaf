using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.contract.standard;

namespace NativeModel.Contract
{
    public class NativeCommodity : Model
    {
        private int _sledCommodityId;
        private string _exchangeMic;
        private int _sledCommodityType;
        private string _sledCommodityCode;
        private ObservableCollection<int> _relateCommodityIds;
        private string _tradeCurrency;
        private string _zoneId;
        private double _contractSize;
        private double _tickSize;
        private int _denominator;
        private string _engName;
        private string _cnName;
        private string _tcName;
        private string _engAcronym;
        private string _cnAcronym;
        private string _tcAcronym;
        private long _activeStartTimestamp;
        private long _activeEndTimestamp;
        private long _createTimestamp;
        private long _lastModityTimestamp;
        private ObservableCollection<SledCommodityConfig> configs;
        
        public int SledCommodityId
        {
            get
            {
                return _sledCommodityId;
            }
            set
            {
                SetProperty(ref _sledCommodityId, value);
            }
        }

        public string ExchangeMic
        {
            get
            {
                return _exchangeMic;
            }
            set
            {
                SetProperty(ref _exchangeMic, value);
            }
        }
        
        public int SledCommodityType
        {
            get
            {
                return _sledCommodityType;
            }
            set
            {
                SetProperty(ref _sledCommodityType, value);
            }
        }

        public string SledCommodityCode
        {
            get
            {
                return _sledCommodityCode;
            }
            set
            {
                SetProperty(ref _sledCommodityCode, value);
            }
        }

        public ObservableCollection<int> RelateCommodityIds
        {
            get
            {
                return _relateCommodityIds;
            }
            set
            {
                SetProperty(ref _relateCommodityIds, value);
            }
        }

        public string TradeCurrency
        {
            get
            {
                return _tradeCurrency;
            }
            set
            {
                SetProperty(ref _tradeCurrency, value);
            }
        }

        public string ZoneId
        {
            get
            {
                return _zoneId;
            }
            set
            {
                SetProperty(ref _zoneId, value);
            }
        }

        public double ContractSize
        {
            get
            {
                return _contractSize;
            }
            set
            {
                SetProperty(ref _contractSize, value);
            }
        }

        public double TickSize
        {
            get
            {
                return _tickSize;
            }
            set
            {
                SetProperty(ref _tickSize, value);
            }
        }

        public int Denominator
        {
            get
            {
                return _denominator;
            }
            set
            {
                SetProperty(ref _denominator, value);
            }
        }

        public string EngName
        {
            get
            {
                return _engName;
            }
            set
            {
                SetProperty(ref _engName, value);
            }
        }

        public string CnName
        {
            get
            {
                return _cnName;
            }
            set
            {
                SetProperty(ref _cnName, value);
            }
        }

        public string TcName
        {
            get
            {
                return _tcName;
            }
            set
            {
                SetProperty(ref _tcName, value);
            }
        }

        public string EngAcronym
        {
            get
            {
                return _engAcronym;
            }
            set
            {
                SetProperty(ref _engAcronym, value);
            }
        }

        public string CnAcronym
        {
            get
            {
                return _cnAcronym;
            }
            set
            {
                SetProperty(ref _cnAcronym, value);
            }
        }

        public string TcAcronym
        {
            get
            {
                return _tcAcronym;
            }
            set
            {
                SetProperty(ref _tcAcronym, value);
            }
        }

        public long ActiveStartTimestamp
        {
            get
            {
                return _activeStartTimestamp;
            }
            set
            {
                SetProperty(ref _activeStartTimestamp, value);
            }
        }

        public long ActiveEndTimestamp
        {
            get
            {
                return _activeEndTimestamp;
            }
            set
            {
                SetProperty(ref _activeEndTimestamp, value);
            }
        }

        public long CreateTimestamp
        {
            get
            {
                return _createTimestamp;
            }
            set
            {
                SetProperty(ref _createTimestamp, value);
            }
        }

        public long LastModityTimestamp
        {
            get
            {
                return _lastModityTimestamp;
            }
            set
            {
                SetProperty(ref _lastModityTimestamp, value);
            }
        }

        public ObservableCollection<SledCommodityConfig> Configs
        {
            get { return configs; }
            set { SetProperty(ref configs, value); }
        }
    }
}
