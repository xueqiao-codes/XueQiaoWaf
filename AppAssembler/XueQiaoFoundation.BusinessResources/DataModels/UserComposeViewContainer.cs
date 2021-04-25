using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 用户组合视图容器
    /// </summary>
    public class UserComposeViewContainer : Model
    {
        public UserComposeViewContainer(long composeGraphId)
        {
            this.ComposeGraphId = composeGraphId;
        }

        /// <summary>
        /// 组合 id
        /// </summary>
        public long ComposeGraphId { get; private set; }
        
        private NativeComposeView userComposeView;
        /// <summary>
        /// 用户组合视图
        /// </summary>
        public NativeComposeView UserComposeView
        {
            get { return userComposeView; }
            set { SetProperty(ref userComposeView, value); }
        }
    }
}
