using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class UserAnyDataRefreshStateChangedArgs : EventArgs
    {
        public UserAnyDataRefreshStateChangedArgs(string loginUserToken, DataRefreshState refreshState)
        {
            this.LoginUserToken = loginUserToken;
            this.RefreshState = refreshState;
        }

        public string LoginUserToken { get; private set; }

        public DataRefreshState RefreshState { get; private set; }
    }

    /// <summary>
    /// 用户的某些数据刷新状态变化 delegate
    /// </summary>
    /// <param name="args"></param>
    public delegate void UserAnyDataRefreshStateChanged(UserAnyDataRefreshStateChangedArgs args);
}
