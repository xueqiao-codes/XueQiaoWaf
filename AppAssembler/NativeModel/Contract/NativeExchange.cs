using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Contract
{
    public class NativeExchange : Model
    {
        private int _sledExchangeId;
        private string _exchangeMic;
        private string _country;
        private string _countryCode;
        private string _operatingMic;
        private string _operatingMicType;
        private string _nameInstitution;
        private string _acronym;
        private string _city;
        private string _website;
        private string _cnName;
        private string _cnAcronym;
        private long _activeStartTimestamp;
        private long _activeEndTimestamp;
        private long _createTimestamp;
        private long _lastModityTimestamp;

        public int SledExchangeId
        {
            get
            {
                return _sledExchangeId;
            }
            set
            {
                SetProperty(ref _sledExchangeId, value);
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

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                SetProperty(ref _country, value);
            }
        }

        public string CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                SetProperty(ref _countryCode, value);
            }
        }

        public string OperatingMic
        {
            get
            {
                return _operatingMic;
            }
            set
            {
                SetProperty(ref _operatingMic, value);
            }
        }
        
        public string OperatingMicType
        {
            get
            {
                return _operatingMicType;
            }
            set
            {
                SetProperty(ref _operatingMicType, value);
            }
        }

        public string NameInstitution
        {
            get
            {
                return _nameInstitution;
            }
            set
            {
                SetProperty(ref _nameInstitution, value);
            }
        }

        public string Acronym
        {
            get
            {
                return _acronym;
            }
            set
            {
                SetProperty(ref _acronym, value);
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                SetProperty(ref _city, value);
            }
        }

        public string Website
        {
            get
            {
                return _website;
            }
            set
            {
                SetProperty(ref _website, value);
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
    }
}
