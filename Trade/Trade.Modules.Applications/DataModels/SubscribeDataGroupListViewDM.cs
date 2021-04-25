using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 订阅数据分组的列表视图 data model
    /// </summary>
    public class SubscribeDataGroupListViewDM : Model
    {
        public SubscribeDataGroupListViewDM(SubscribeDataGroup group)
        {
            this.Group = group;
        }

        // 分组
        public SubscribeDataGroup Group { get; private set; }
        
        private object listView;
        // 列表视图
        public object ListView
        {
            get { return listView; }
            set { SetProperty(ref listView, value); }
        }
    }
}
