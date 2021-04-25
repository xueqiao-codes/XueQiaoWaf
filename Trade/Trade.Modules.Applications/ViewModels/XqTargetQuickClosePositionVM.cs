using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetQuickClosePositionVM : ViewModel<IXqTargetQuickClosePositionView>
    {
        [ImportingConstructor]
        protected XqTargetQuickClosePositionVM(IXqTargetQuickClosePositionView view) : base(view)
        {
            ToRemainEditItems = new ObservableCollection<XqTargetClosePositionRemainEditItem>();
        }

        private TargetPositionDataModel xqTargetPositionItem;
        public TargetPositionDataModel XqTargetPositionItem
        {
            get { return xqTargetPositionItem; }
            set { SetProperty(ref xqTargetPositionItem, value); }
        }

        private int netPosition;
        // 净仓数量
        public int NetPosition
        {
            get { return netPosition; }
            set { SetProperty(ref netPosition, value); }
        }

        /// <summary>
        /// 要保留净仓的编辑项列表
        /// </summary>
        public ObservableCollection<XqTargetClosePositionRemainEditItem> ToRemainEditItems { get; private set; }

        private ICommand submitClosePositionCmd;
        public ICommand SubmitClosePositionCmd
        {
            get { return submitClosePositionCmd; }
            set { SetProperty(ref submitClosePositionCmd, value); }
        }

    }
}
