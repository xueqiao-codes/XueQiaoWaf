using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dragablz
{
    /// <summary>
    /// Helper class to create view models, particularly for tool/MDI windows.
    /// </summary>
    public class HeaderedItemViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private object _header;
        private object _content;

        public HeaderedItemViewModel()
        {
        }

        public HeaderedItemViewModel(object header, object content, bool isSelected = false)
        {
            _header = header;
            _content = content;
            _isSelected = isSelected;
        }

        public object Header
        {
            get { return _header; }
            set
            {
                if (_header == value) return;
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        public object Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
