using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class ComponentTabSelectionItem : Model
    {
        public ComponentTabSelectionItem(int componentType)
        {
            this.ComponentType = componentType;
        }
        
        public int ComponentType { get; private set; }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }
    }
}
