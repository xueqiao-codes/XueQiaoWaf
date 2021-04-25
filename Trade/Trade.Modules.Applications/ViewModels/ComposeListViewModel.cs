using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ComposeListViewModel : ViewModel<IComposeListView>
    {
        private readonly SubscribeComposeService subscribeComposeService;

        [ImportingConstructor]
        protected ComposeListViewModel(IComposeListView view,
            SubscribeComposeService subscribeComposeService) : base(view)
        {
            this.AddibleCustomGroups = new ObservableCollection<AddSubscribeDataToGroupDM>();
            CollectionChangedEventManager.AddHandler(AddibleCustomGroups, AddibleCustomGroupCollectionChanged);

            this.subscribeComposeService = subscribeComposeService;
            var syncComposeItems = new SynchronizingCollection<SubscribeComposeDataModel, SubscribeComposeDataModel>(subscribeComposeService.Composes, i => i);
            ComposeCollectionView = CollectionViewSource.GetDefaultView(syncComposeItems) as ListCollectionView;

            ConfigComposeCollectionView();
            InvalidateListGlobalFilter();

            InavlidateExistAddibleCustomGroups();
        }
        
        public UIElement SubscribeItemElement(object subscribeItemData) => ViewCore.SubscribeItemElement(subscribeItemData);
        
        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }

        public ListCollectionView ComposeCollectionView { get; private set; }
        
        private SubscribeDataGroup groupTab;
        // 当前列表的分组 tab 
        public SubscribeDataGroup GroupTab
        {
            get { return groupTab; }
            set
            {
                if (SetProperty(ref groupTab, value))
                {
                    InvalidateListGlobalFilter();
                }
            }
        }

        private long presentSubAccountId;
        // 子账户 id
        public long PresentSubAccountId
        {
            get { return presentSubAccountId; }
            set
            {
                if (SetProperty(ref presentSubAccountId, value))
                {
                    InvalidateListGlobalFilter();
                }
            }
        }

        /// <summary>
        /// 可添加订阅项的分组
        /// </summary>
        public ObservableCollection<AddSubscribeDataToGroupDM> AddibleCustomGroups { get; private set; }

        private bool existAddibleCustomGroups;
        /// <summary>
        /// 是否存在可添加订阅项的分组
        /// </summary>
        public bool ExistAddibleCustomGroups
        {
            get { return existAddibleCustomGroups; }
            private set { SetProperty(ref existAddibleCustomGroups, value); }
        }

        private ICommand toConfigureListDisplayColumnsCmd;
        public ICommand ToConfigureListDisplayColumnsCmd
        {
            get { return toConfigureListDisplayColumnsCmd; }
            set { SetProperty(ref toConfigureListDisplayColumnsCmd, value); }
        }

        private ICommand toApplyListDefaultDisplayColumnsCmd;
        public ICommand ToApplyListDefaultDisplayColumnsCmd
        {
            get { return toApplyListDefaultDisplayColumnsCmd; }
            set { SetProperty(ref toApplyListDefaultDisplayColumnsCmd, value); }
        }

        private SelectedComposesOperateCommands selectedItemsOptCommands;
        public SelectedComposesOperateCommands SelectedItemsOptCommands
        {
            get { return selectedItemsOptCommands; }
            set { SetProperty(ref selectedItemsOptCommands, value); }
        }

        private ICommand clickItemTargetKeyRelatedColumnCmd;
        /// <summary>
        /// 点击项目的唯一标识相关列 command
        /// cmd param: <see cref="SubscribeComposeDataModel"/>类型
        /// </summary>
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
        }

        private ICommand clickItemPriceRelatedColumnCmd;
        /// <summary>
        /// 点击项目的价格相关列 command
        /// cmd param: 数组类型。arg0:<see cref="SubscribeComposeDataModel"/>类型，arg1: <see cref="double"/>类型
        /// </summary>
        public ICommand ClickItemPriceRelatedColumnCmd
        {
            get { return clickItemPriceRelatedColumnCmd; }
            set { SetProperty(ref clickItemPriceRelatedColumnCmd, value); }
        }

        private void AddibleCustomGroupCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InavlidateExistAddibleCustomGroups();
        }

        private void InavlidateExistAddibleCustomGroups()
        {
            this.ExistAddibleCustomGroups = (this.AddibleCustomGroups.Count > 0);
        }

        private void ConfigComposeCollectionView()
        {
            var listView = ComposeCollectionView;
            if (listView == null) return;

            // 组合列表的排序：未过期的在上，过期的在下
            listView.SortDescriptions.Add(new SortDescription { PropertyName = nameof(SubscribeComposeDataModel.IsXqTargetExpired), Direction = ListSortDirection.Ascending });
            listView.SortDescriptions.Add(new SortDescription { PropertyName = nameof(SubscribeComposeDataModel.CreateTimestamp), Direction = ListSortDirection.Ascending });
            listView.LiveSortingProperties.Add(nameof(SubscribeComposeDataModel.IsXqTargetExpired));
            listView.LiveSortingProperties.Add(nameof(SubscribeComposeDataModel.CreateTimestamp));
            listView.LiveSortingProperties.Add(nameof(SubscribeComposeDataModel.XqTargetName));
            listView.IsLiveSorting = true;
        }

        private void InvalidateListGlobalFilter()
        {
            var listView = ComposeCollectionView;
            if (listView == null) return;

            listView.Filter = new Predicate<object>(obj =>
            {
                var subItem = obj as SubscribeComposeDataModel;
                if (subItem == null) return false;
                if (subItem.SubscribeGroupKey != SubscribeComposeDataModel.SharedListComposeGroupKey) return false;

                var isPassGroupTabFilter = false;
                var groupTab = this.GroupTab;
                if (groupTab != null)
                {
                    switch (groupTab.GroupType)
                    {
                        case SubscribeDataGroupType.OnTrading:
                            isPassGroupTabFilter = subItem.OnTradingSubAccountIds?.Contains(PresentSubAccountId) ?? false;
                            break;
                        case SubscribeDataGroupType.ExistPosition:
                            isPassGroupTabFilter = subItem.ExistPositionSubAccountIds?.Contains(PresentSubAccountId) ?? false;
                            break;
                        case SubscribeDataGroupType.IsExpired:
                            isPassGroupTabFilter = (subItem.IsXqTargetExpired == true);
                            break;
                        case SubscribeDataGroupType.Custom:
                            isPassGroupTabFilter = subItem.CustomGroupKeys?.Contains(groupTab.GroupKey ?? "") ?? false;
                            break;
                        default:
                            isPassGroupTabFilter = true;
                            break;
                    }
                }
                if (!isPassGroupTabFilter) return isPassGroupTabFilter;
                return true;
            });

            var liveFilteringProps = new string[]
            {
                nameof(SubscribeComposeDataModel.CustomGroupKeys),
                nameof(SubscribeComposeDataModel.OnTradingSubAccountIds),
                nameof(SubscribeComposeDataModel.ExistPositionSubAccountIds),
                nameof(SubscribeComposeDataModel.IsXqTargetExpired)
            };
            foreach (var liveFilteringProp in liveFilteringProps)
            {
                if (!listView.LiveFilteringProperties.Contains(liveFilteringProp))
                {
                    listView.LiveFilteringProperties.Add(liveFilteringProp);
                }
            }
            listView.IsLiveFiltering = true;
            listView.Refresh();
        }
    }
}
