using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetPositionItemDialogTitleVM : ViewModel<IXqTargetPositionItemDialogTitleView>
    {
        [ImportingConstructor]
        protected XqTargetPositionItemDialogTitleVM(IXqTargetPositionItemDialogTitleView view) : base(view)
        {
        }
        
        private TargetPositionDataModel _XqTargetPositionItem;
        public TargetPositionDataModel XqTargetPositionItem
        {
            get { return _XqTargetPositionItem; }
            set { SetProperty(ref _XqTargetPositionItem, value); }
        }

        private string titlePrefix;
        /// <summary>
        /// 标题前缀
        /// </summary>
        public string TitlePrefix
        {
            get { return titlePrefix; }
            set { SetProperty(ref titlePrefix, value); }
        }
    }
}
