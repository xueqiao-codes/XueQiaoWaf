using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.UI.Controls
{
    public class DayDatePickerSelectionChangedEventArgs : RoutedEventArgs
    {
        public DayDatePickerSelectionChangedEventArgs(RoutedEvent eventId, DateTime? oldValue, DateTime? newValue) :
            base(eventId)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public DateTime? OldValue { get; }
        public DateTime? NewValue { get; }
    }
}
