using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Contract
{
    public class NativeContract : Model
    {
        private int _sledContractId;
        private int _sledCommodityId;
        private string _sledContractCode;
        private ObservableCollection<int> _relateContractIds;
        private string _sledTag;
        private string _contractEngName;
        private string _contractCnName;
        private string _contractTcName;
        private long _contractExpDate;
        private long _lastTradeDate;
        private long _firstNoticeDate;
        private bool _subscribeXQQuote;
        private long _activeStartTimestamp;
        private long _activeEndTimestamp;
        private long _createTimestamp;
        private long _lastModityTimestamp;
        private bool _isDisabled;
        private bool _isExpired;

        public int SledContractId
        {
            get
            {
                return _sledContractId;
            }
            set
            {
                SetProperty(ref _sledContractId, value);
            }
        }

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

        public string SledContractCode
        {
            get
            {
                return _sledContractCode;
            }
            set
            {
                SetProperty(ref _sledContractCode, value);
            }
        }

        public ObservableCollection<int> RelateContractIds
        {
            get
            {
                return _relateContractIds;
            }
            set
            {
                SetProperty(ref _relateContractIds, value);
            }
        }

        public string SledTag
        {
            get
            {
                return _sledTag;
            }
            set
            {
                SetProperty(ref _sledTag, value);
            }
        }

        public string ContractEngName
        {
            get
            {
                return _contractEngName;
            }
            set
            {
                SetProperty(ref _contractEngName, value);
            }
        }

        public string ContractCnName
        {
            get
            {
                return _contractCnName;
            }
            set
            {
                SetProperty(ref _contractCnName, value);
            }
        }

        public string ContractTcName
        {
            get
            {
                return _contractTcName;
            }
            set
            {
                SetProperty(ref _contractTcName, value);
            }
        }

        public long ContractExpDate
        {
            get
            {
                return _contractExpDate;
            }
            set
            {
                SetProperty(ref _contractExpDate, value);
            }
        }

        public long LastTradeDate
        {
            get
            {
                return _lastTradeDate;
            }
            set
            {
                SetProperty(ref _lastTradeDate, value);
            }
        }

        public long FirstNoticeDate
        {
            get
            {
                return _firstNoticeDate;
            }
            set
            {
                SetProperty(ref _firstNoticeDate, value);
            }
        }

        public bool SubscribeXQQuote
        {
            get { return _subscribeXQQuote; }
            set { SetProperty(ref _subscribeXQQuote, value); }
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

        public bool IsDisabled
        {
            get
            {
                return _isDisabled;
            }
            set
            {
                SetProperty(ref _isDisabled, value);
            }
        }

        public bool IsExpired
        {
            get
            {
                return _isExpired;
            }
            set
            {
                SetProperty(ref _isExpired, value);
            }
        }
    }
}
