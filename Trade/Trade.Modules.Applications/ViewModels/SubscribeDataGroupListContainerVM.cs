using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubscribeDataGroupListContainerVM : ViewModel<ISubscribeDataGroupListContainerView>
    {
        [ImportingConstructor]
        protected SubscribeDataGroupListContainerVM(ISubscribeDataGroupListContainerView view) : base(view)
        {
            GroupListViewsDragDropContextId = UUIDHelper.CreateUUIDString(false);
            GroupListViewsDragHandler = new SubscribeDataGroupListDragHandler();
            GroupListViewsDropHandler = new SubscribeDataGroupListDropHandler();
            DataGroupListViews = new ObservableCollection<SubscribeDataGroupListViewDM>();
        }
        
        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public UIElement GroupItemElement(object groupItemData) => ViewCore.GroupItemElement(groupItemData);

        public void Scroll2GroupItem(object groupItemData) => ViewCore.Scroll2GroupItem(groupItemData);

        public UIElement AddGroupButton => ViewCore.AddGroupButton;

        /// <summary>
        /// 分组列表 item 拖拽上下文标识，用于防止别的列表项拖拽到当前列表
        /// </summary>
        public string GroupListViewsDragDropContextId { get; private set; }

        public SubscribeDataGroupListDragHandler GroupListViewsDragHandler { get; private set; }

        public SubscribeDataGroupListDropHandler GroupListViewsDropHandler { get; private set; }

        /// <summary>
        /// 数据的所有分组
        /// </summary>
        public ObservableCollection<SubscribeDataGroupListViewDM> DataGroupListViews { get; private set; }
        
        private SubscribeDataGroupListViewDM selectedGroupListViewItem;
        /// <summary>
        /// 选中的分组
        /// </summary>
        public SubscribeDataGroupListViewDM SelectedGroupListViewItem
        {
            get { return selectedGroupListViewItem; }
            set { SetProperty(ref selectedGroupListViewItem, value); }
        }

        private int frozenGroupNum;
        /// <summary>
        /// 固定不动的分组数量
        /// </summary>
        public int FrozenGroupNum
        {
            get { return frozenGroupNum; }
            set
            {
                SetProperty(ref frozenGroupNum, value);
                if (GroupListViewsDragHandler != null) GroupListViewsDragHandler.FrozenGroupNum = frozenGroupNum;
                if (GroupListViewsDropHandler != null) GroupListViewsDropHandler.FrozenGroupNum = frozenGroupNum;
            }
        }

        private ICommand toNewGroupTabCmd;
        /// <summary>
        /// 新建 tag tab。param type<see cref="SubscribeDataGroupListViewDM"/>，参考该值确定新建项的添加位置
        /// </summary>
        public ICommand ToNewGroupTabCmd
        {
            get { return toNewGroupTabCmd; }
            set { SetProperty(ref toNewGroupTabCmd, value); }
        }

        private ICommand toRemoveGroupTabCmd;
        /// <summary>
        /// 删除 tag tab。param type<see cref="SubscribeDataGroupListViewDM"/>
        /// </summary>
        public ICommand ToRemoveGroupTabCmd
        {
            get { return toRemoveGroupTabCmd; }
            set { SetProperty(ref toRemoveGroupTabCmd, value); }
        }

        private ICommand toRenameGroupTabCmd;
        /// <summary>
        /// 重命名 tag tab。param type<see cref="SubscribeDataGroupListViewDM"/>
        /// </summary>
        public ICommand ToRenameGroupTabCmd
        {
            get { return toRenameGroupTabCmd; }
            set { SetProperty(ref toRenameGroupTabCmd, value); }
        }
    }

    public class SubscribeDataGroupListDragHandler : DefaultDragHandler
    {
        public int FrozenGroupNum { get; set; }
        
        public override bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo.SourceIndex < this.FrozenGroupNum)
                return false;
            return base.CanStartDrag(dragInfo);
        }
    }

    public class SubscribeDataGroupListDropHandler : DefaultDropHandler
    {
        public int FrozenGroupNum { get; set; }

        public override void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.InsertIndex < this.FrozenGroupNum)
            {
                dropInfo.NotHandled = true;
            }
            else
            {
                base.DragOver(dropInfo);
            }
        }

        public override void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.InsertIndex < this.FrozenGroupNum)
            {
                dropInfo.NotHandled = true;
            }
            else
            {
                base.Drop(dropInfo);
            }
        }
    }
}
