using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BolapanControl.ItemsFilter
{
    public delegate void FilterControlStateChanged(FilterControlStateChangedArgs args);

    public class FilterControlStateChangedArgs
    {
        public FilterControlStateChangedArgs(FilterControl filterControl, FilterControlState oldState, FilterControlState newState)
        {
            this.FilterControl = filterControl;
            this.OldState = oldState;
            this.NewState = newState;
        }

        public readonly FilterControl FilterControl;

        public readonly FilterControlState OldState;

        public readonly FilterControlState NewState;
    }
}
