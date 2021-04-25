using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public interface IComposeDetailDataModel : INotifyPropertyChanged
    {
        NativeComposeGraph BasicComposeGraph { get; }

        ObservableCollection<ComposeLegDetail> DetailLegs { get; }
    }
}
