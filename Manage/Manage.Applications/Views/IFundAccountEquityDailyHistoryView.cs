﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Manage.Applications.Views
{
    public interface IFundAccountEquityDailyHistoryView : IView
    {
        /// <summary>
        /// Get the window the view display in.
        /// </summary>
        object DisplayInWindow { get; }
    }
}