using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IUserSubscribeDataDao
    {
        UserSubscribeDataTree GetUserSubscribeData(LandingInfo landingInfo, out int? dataVersion);

        int? SaveSubscribeData(LandingInfo landingInfo, UserSubscribeDataTree userSubscribeContractTree, int dataVersion);
    }
}
