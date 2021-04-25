using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Trade
{
    public class NativeComposeGraph : Model
    {
        private int _createSubUserId;
        private string _formular;
        private ObservableCollection<NativeComposeLeg> _legs;
        private string composeGraphKey;
        private long createTimestamp;
        
        public NativeComposeGraph(long composeGraphId)
        {
            this.ComposeGraphId = composeGraphId;
        }

        public long ComposeGraphId { private set; get; }
        
        public int CreateSubUserId
        {
            get
            {
                return _createSubUserId;
            }
            set
            {
                SetProperty(ref _createSubUserId, value);
            }
        }

        public string Formular
        {
            get
            {
                return _formular;
            }
            set
            {
                SetProperty(ref _formular, value);
            }
        }

        public ObservableCollection<NativeComposeLeg> Legs
        {
            get
            {
                return _legs;
            }
            set
            {
                SetProperty(ref _legs, value);
            }
        }
        
        public string ComposeGraphKey
        {
            get { return composeGraphKey; }
            set { SetProperty(ref composeGraphKey, value); }
        }

        public long CreateTimestamp
        {
            get { return createTimestamp; }
            set { SetProperty(ref createTimestamp, value); }
        }

        public void UpdateCreateTimestamp(long createTimestamp)
        {
            this.CreateTimestamp = createTimestamp;
        }
    }
}
