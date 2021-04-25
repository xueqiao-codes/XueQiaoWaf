using ContainerShell.Applications.Views;
using Dragablz;
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
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class WorkspaceTabablzControlVM : ViewModel<IWorkspaceTabablzControl>
    {
        [ImportingConstructor]
        public WorkspaceTabablzControlVM(IWorkspaceTabablzControl view) : base(view)
        {
            WorkspaceItems = new ObservableCollection<TabWorkspaceItemDataModel>();
        }
        
        public ObservableCollection<TabWorkspaceItemDataModel> WorkspaceItems { get; }

        private TabWorkspaceItemDataModel activeWorkspaceItem;
        public TabWorkspaceItemDataModel ActiveWorkspaceItem
        {
			get { return activeWorkspaceItem; }            
			set { SetProperty(ref activeWorkspaceItem, value); }
		}

        private IInterTabClient interTabClient;
        public IInterTabClient InterTabClient
        {
            get { return interTabClient; }
            set { SetProperty(ref interTabClient, value); }
        }

        /// <summary>
        /// tab control 的 tab 拆分控制 key。从相同 key 的tab control 拆分出去的 tab 可以合并回至该 tab control
        /// </summary>
        private string interTabPartitionKey;
        public string InterTabPartitionKey
        {
            get { return interTabPartitionKey; }
            set { SetProperty(ref interTabPartitionKey, value); }
        }
        
        private ICommand newCommand;
        public ICommand NewCommand
		{            
			get { return newCommand; }
			set { SetProperty(ref newCommand, value); }
		}

        private ICommand splitWorkspaceAsWindowCmd;
        // 拆分某个工作空间为独立窗口 command
        public ICommand SplitWorkspaceAsWindowCmd
        {
            get { return splitWorkspaceAsWindowCmd; }
            set { SetProperty(ref splitWorkspaceAsWindowCmd, value); }
        }

        private ICommand renameWorkspaceCmd;
        // 重命名某个工作空间
        public ICommand RenameWorkspaceCmd
        {
            get { return renameWorkspaceCmd; }
            set { SetProperty(ref renameWorkspaceCmd, value); }
        }

        /// <summary>
        /// （顺序从左到右）固定 tab 数目
        /// </summary>
        private int fixedItemCount;
        public int FixedItemCount
        {
            get { return fixedItemCount; }
            set { SetProperty(ref fixedItemCount, value); }
        }

        private bool isEmbedInWindowCaption;
        /// <summary>
        /// 是否嵌入到窗体头部
        /// </summary>
        public bool IsEmbedInWindowCaption
        {
            get { return isEmbedInWindowCaption; }
            set { SetProperty(ref isEmbedInWindowCaption, value); }
        }

        private ChromeWindowCaptionDataHolder embedInWindowCaptionDataHolder;
        /// <summary>
        /// 当嵌入到窗体头部时，窗体头部数据的 holder
        /// </summary>
        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder
        {
            get { return embedInWindowCaptionDataHolder; }
            set { SetProperty(ref embedInWindowCaptionDataHolder, value); }
        }

        /// <summary>
        /// 工作空间将要关闭事件
        /// <arg1>当前工作空间项</arg1>
        /// <arg2>可取消的句柄</arg2>
        /// </summary>
        public event Action<TabWorkspaceItemDataModel, CancelEventArgs> WorkspaceItemClosing;

        /// <summary>
        /// 工作空间项顺序改变事件
        /// <arg1>当前工作空间项</arg1>
        /// <arg2>新的顺序值</arg2>
        /// <arg3>旧的顺序值</arg3>
        /// </summary>
        public event Action<TabWorkspaceItemDataModel, int, int> WorkspaceItemLogicalIndexChanged;

        /// <summary>
        /// 工作空间列表的 tab 控件
        /// </summary>
        public object WorkspaceTabControl => ViewCore.WorkspaceTabControl;

        /// <summary>
        /// 所在窗体
        /// </summary>
        public object ContainerWindow => ViewCore.ContainerWindow;

        /// <summary>
        /// 某个工作空间项的显示元素
        /// </summary>
        /// <param name="workspaceItemData"></param>
        /// <returns></returns>
        public UIElement WorkspaceItemElement(object workspaceItemData) => ViewCore.WorkspaceItemElement(workspaceItemData);


        /// <summary>
        /// 发布工作空间关闭的事件
        /// </summary>
        /// <param name="closingItem">要关闭的项</param>
        /// <param name="e">可取消的句柄</param>
        public void PublishWorkspaceItemClosing(object closingItem, CancelEventArgs e)
        {
            var currentItem = closingItem as TabWorkspaceItemDataModel;
            if (currentItem != null)
            {
                WorkspaceItemClosing?.Invoke(currentItem, e);
            }
        }

        /// <summary>
        /// 发布工作空间的顺序改变的事件
        /// </summary>
        /// <param name="item">改变的项</param>
        /// <param name="newIndexValue">新的顺序值</param>
        /// <param name="oldIndexValue">就的顺序值</param>
        public void PublishWorkspaceItemLogicalIndexChanged(object optItem, int newIndexValue, int oldIndexValue)
        {
            var currentItem = optItem as TabWorkspaceItemDataModel;
            if (currentItem != null)
            {
                currentItem.TabWorkspace.Order = newIndexValue;
                WorkspaceItemLogicalIndexChanged?.Invoke(currentItem, newIndexValue, oldIndexValue);
            }
        }
    }
}
