using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    /// <summary>
    /// `组合列表组件`内容视图
    /// </summary>
    public interface ISubscribeDataGroupListContainerView : IView
    {
        object DisplayInWindow { get; }
        
        /// <summary>
        /// 分组项的显示元素
        /// </summary>
        /// <param name="groupItemData"></param>
        /// <returns></returns>
        UIElement GroupItemElement(object groupItemData);

        /// <summary>
        /// 滚动到分组项
        /// </summary>
        /// <param name="groupItemData"></param>
        void Scroll2GroupItem(object groupItemData);

        /// <summary>
        /// 获取添加分组按钮
        /// </summary>
        UIElement AddGroupButton { get; }
    }
}
