using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Media;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.DataModels
{
    public class ContainerShellTabNodeItem : AppModuleTabNode
    {
        public ContainerShellTabNodeItem(ContainerShellTabNodeType tabNodeType)
        {
            this.TabNodeType = tabNodeType;
        }

        public ContainerShellTabNodeType TabNodeType { get; private set; }
    }
}
