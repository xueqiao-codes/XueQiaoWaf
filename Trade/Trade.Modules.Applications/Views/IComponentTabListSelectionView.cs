using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IComponentTabListSelectionView : IView
    {
        void ShowPopup(object targetElement);

        void Close();
    }
}
