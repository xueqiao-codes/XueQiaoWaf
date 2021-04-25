using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 添加订阅数据到分组 data model
    /// </summary>
    public class AddSubscribeDataToGroupDM
    {
        public AddSubscribeDataToGroupDM(SubscribeDataGroup group, ICommand addToGroupCmd)
        {
            this.Group = group;
            this.AddToGroupCmd = addToGroupCmd;
        }

        public SubscribeDataGroup Group { get; private set; }

        public ICommand AddToGroupCmd { get; private set; }
    }
}
