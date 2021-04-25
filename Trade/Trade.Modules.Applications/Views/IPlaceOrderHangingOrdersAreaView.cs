﻿using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    /// <summary>
    /// 下单页面挂单列表区域视图
    /// </summary>
    public interface IPlaceOrderHangingOrdersAreaView : IView
    {
        void ResetOrderListColumnsByPresentTarget(ClientXQOrderTargetType targetType);
    }
}
