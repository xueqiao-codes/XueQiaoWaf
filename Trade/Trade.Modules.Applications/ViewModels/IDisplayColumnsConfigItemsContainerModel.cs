using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    public interface IDisplayColumnsConfigItemsContainerModel<T> : INotifyPropertyChanged
    {
        T SelectedConfigItem { get; }

        ObservableCollection<T> ColumnConfigItems { get; }
    }
}
