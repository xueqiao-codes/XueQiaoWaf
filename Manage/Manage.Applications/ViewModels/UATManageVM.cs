using Manage.Applications.DataModels;
using Manage.Applications.Services;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using System.Waf.Foundation;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UATManageVM : ViewModel<IUATManageView>
    {
        [ImportingConstructor]
        protected UATManageVM(IUATManageView view,
            UATPAContractSummaryService UATPAContractSummaryService) : base(view)
        {
            this.UATPAContractSummaryService = UATPAContractSummaryService;
            ViewTabItems = new ObservableCollection<UATManageViewTabItem>();
        }
        
        private ModuleLockStatusDM moduleLockStatus;
        public ModuleLockStatusDM ModuleLockStatus
        {
            get { return moduleLockStatus; }
            set { SetProperty(ref moduleLockStatus, value); }
        }

        public UATPAContractSummaryService UATPAContractSummaryService { get; private set; }

        // 视图的 tab 列表
        public ObservableCollection<UATManageViewTabItem> ViewTabItems { get; private set; }

        private UATManageViewTabItem selectedViewTabItem;
        // 选中的 tab
        public UATManageViewTabItem SelectedViewTabItem
        {
            get { return selectedViewTabItem; }
            set { SetProperty(ref selectedViewTabItem, value); }
        }
    }

    /// <summary>
    /// 未分配成交管理页面的 tab 类型
    /// </summary>
    public enum UATManageViewTabType
    {
        AssignTab = 1,  // 分配 tab
        PreviewTab = 2, // 预览tab
    }

    /// <summary>
    /// 未分配成交管理页面的 tab 项
    /// </summary>
    public class UATManageViewTabItem : Model
    {
        public UATManageViewTabItem(UATManageViewTabType tabType)
        {
            this.TabType = tabType;
        }

        public UATManageViewTabType TabType { get; private set; }

        private object contentView;
        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }
    }
}
