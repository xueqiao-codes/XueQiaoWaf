using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace XueQiaoFoundation.UI.Controls
{

    [TemplatePart(Name = ElementDayPreviousButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementSelectedDayTitleButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementDayNextButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementCalendarPopup, Type = typeof(Popup))]
    [TemplatePart(Name = ElementCalendar, Type = typeof(Calendar))]
    public class DayDatePicker : Control
    {
        private const string ElementDayPreviousButton = "PART_DayPreviousButton";
        private const string ElementSelectedDayTitleButton = "PART_SelectedDayTitleButton";
        private const string ElementDayNextButton = "PART_DayNextButton";
        private const string ElementCalendarPopup = "PART_CalendarPopup";
        private const string ElementCalendar = "PART_Calendar";
        

        public static readonly DependencyProperty FirstDayOfWeekProperty = DatePicker.FirstDayOfWeekProperty.AddOwner(typeof(DayDatePicker));
        public static readonly DependencyProperty IsTodayHighlightedProperty = DatePicker.IsTodayHighlightedProperty.AddOwner(typeof(DayDatePicker));
        public static readonly DependencyProperty IsDropDownOpenProperty = DatePicker.IsDropDownOpenProperty.AddOwner(typeof(DayDatePicker), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty DisplayDateStartProperty = DatePicker.DisplayDateStartProperty.AddOwner(
            typeof(DayDatePicker),
            new FrameworkPropertyMetadata(default(DateTime?), OnDisplayDateStartChanged));

        public static readonly DependencyProperty DisplayDateEndProperty = DatePicker.DisplayDateEndProperty.AddOwner(
            typeof(DayDatePicker),
            new FrameworkPropertyMetadata(default(DateTime?), OnDisplayDateEndChanged));

        public static readonly DependencyProperty SelectedDateFormatProperty = DatePicker.SelectedDateFormatProperty.AddOwner(
            typeof(DayDatePicker),
            new FrameworkPropertyMetadata(DatePickerFormat.Short, OnSelectedDateFormatChanged));
        
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedDateChanged",
            RoutingStrategy.Direct,
            typeof(EventHandler<DayDatePickerSelectionChangedEventArgs>),
            typeof(DayDatePicker));

        public static readonly DependencyProperty SelectedDateProperty = DatePicker.SelectedDateProperty.AddOwner(
            typeof(DayDatePicker),
            new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", 
                typeof(string), 
                typeof(DayDatePicker), 
                new PropertyMetadata(string.Empty, OnWatermarkChanged));

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture",
            typeof(System.Globalization.CultureInfo),
            typeof(DayDatePicker),
            new PropertyMetadata(null, OnCultureChanged));



        static DayDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayDatePicker), new FrameworkPropertyMetadata(typeof(DayDatePicker)));
        }

        public DayDatePicker()
        {
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, this.OutsideCapturedElementHandler);
        }

        /// <summary>
        ///     Occurs when the <see cref="SelectedDate" /> property is changed.
        /// </summary>
        public event EventHandler<DayDatePickerSelectionChangedEventArgs> SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }
        
        /// <summary>
        ///     Gets or sets the last date to be displayed.
        /// </summary>
        /// <returns>The last date to display.</returns>
        public DateTime? DisplayDateEnd
        {
            get { return (DateTime?)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the first date to be displayed.
        /// </summary>
        /// <returns>The first date to display.</returns>
        public DateTime? DisplayDateStart
        {
            get { return (DateTime?)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the day that is considered the beginning of the week.
        /// </summary>
        /// <returns>
        ///     A <see cref="DayOfWeek" /> that represents the beginning of the week. The default is the
        ///     <see cref="System.Globalization.DateTimeFormatInfo.FirstDayOfWeek" /> that is determined by the current culture.
        /// </returns>
        public DayOfWeek FirstDayOfWeek
        {
            get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
            set { SetValue(FirstDayOfWeekProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the drop-down for a <see cref="TimePickerBase"/> box is currently
        ///         open.
        /// </summary>
        /// <returns>true if the drop-down is open; otherwise, false. The default is false.</returns>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the format that is used to display the selected date.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(DatePickerFormat.Short)]
        public DatePickerFormat SelectedDateFormat
        {
            get { return (DatePickerFormat)GetValue(SelectedDateFormatProperty); }
            set { SetValue(SelectedDateFormatProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value that indicates whether the current date will be highlighted.
        /// </summary>
        /// <returns>true if the current date is highlighted; otherwise, false. The default is true. </returns>
        public bool IsTodayHighlighted
        {
            get { return (bool)GetValue(IsTodayHighlightedProperty); }
            set { SetValue(IsTodayHighlightedProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the currently selected date.
        /// </summary>
        /// <returns>
        ///     The date currently selected. The default is null.
        /// </returns>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating the culture to be used in string formatting operations.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        public System.Globalization.CultureInfo Culture
        {
            get { return (System.Globalization.CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        private Button _dayPreviousButton;
        private Button _selectedDayTitleButton;
        private Button _dayNextButton;
        private Popup _calendarPopup;
        private Calendar _calendar;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dayPreviousButton = GetTemplateChild(ElementDayPreviousButton) as Button;
            _selectedDayTitleButton = GetTemplateChild(ElementSelectedDayTitleButton) as Button;
            _dayNextButton = GetTemplateChild(ElementDayNextButton) as Button;
            _calendarPopup = GetTemplateChild(ElementCalendarPopup) as Popup;
            _calendar = GetTemplateChild(ElementCalendar) as Calendar;

            ApplyBindings();
            ApplySelectedDate();
        }
        
        protected void ApplyBindings()
        {
            if (_dayPreviousButton != null)
            {
                _dayPreviousButton.Click -= OnDayPreviousButtonClick;
                _dayPreviousButton.Click += OnDayPreviousButtonClick;
            }

            if (_selectedDayTitleButton != null)
            {
                _selectedDayTitleButton.Click -= OnSelectedDayTitleButtonClick;
                _selectedDayTitleButton.Click += OnSelectedDayTitleButtonClick;
            }

            if (_dayNextButton != null)
            {
                _dayNextButton.Click -= OnDayNextButtonClick;
                _dayNextButton.Click += OnDayNextButtonClick;
            }

            if (_calendarPopup != null)
            {
                _calendarPopup.SetBinding(Popup.IsOpenProperty, GetBinding(IsDropDownOpenProperty));
            }

            if (_calendar != null)
            {
                _calendar.SetBinding(Calendar.DisplayDateStartProperty, GetBinding(DisplayDateStartProperty));
                _calendar.SetBinding(Calendar.DisplayDateEndProperty, GetBinding(DisplayDateEndProperty));
                _calendar.SetBinding(Calendar.FirstDayOfWeekProperty, GetBinding(FirstDayOfWeekProperty));
                _calendar.SetBinding(Calendar.IsTodayHighlightedProperty, GetBinding(IsTodayHighlightedProperty));
                _calendar.SetBinding(FlowDirectionProperty, GetBinding(FlowDirectionProperty));
                _calendar.SelectedDatesChanged -= OnCalendarSelectedDateChanged;
                _calendar.SelectedDatesChanged += OnCalendarSelectedDateChanged;
            }
        }

        protected Binding GetBinding(DependencyProperty property)
        {
            return new Binding(property.Name) { Source = this };
        }

        protected virtual void OnSelectedDateChanged(DayDatePickerSelectionChangedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected virtual void ApplySelectedDate()
        {
            var selectedDate = this.SelectedDate;

            var calendar = this._calendar;
            if (calendar != null && calendar.SelectedDate != selectedDate)
            {
                calendar.SelectedDatesChanged -= OnCalendarSelectedDateChanged;
                calendar.SelectedDate = selectedDate;
                calendar.SelectedDatesChanged += OnCalendarSelectedDateChanged;
            }

            var selectedDayButn = this._selectedDayTitleButton;
            if (selectedDayButn != null)
            {
                selectedDayButn.Content = selectedDate != null ? GetFormatSelectedDate() : Watermark;
            }

            InvalidateDayNavigateButtonEnabled();
        }

        private void InvalidateDayNavigateButtonEnabled()
        {
            var selectedDate = this.SelectedDate;
            var displayDateStart = this.DisplayDateStart;
            var displayDateEnd = this.DisplayDateEnd;
            var dayPreviousButn = this._dayPreviousButton;
            var dayNextButn = this._dayNextButton;

            var navigateButtonEnabled = selectedDate != null;
            var dayPreviousEnabled = navigateButtonEnabled;
            var dayNextEnabled = navigateButtonEnabled;
            
            if (selectedDate != null && displayDateStart != null)
            {
                dayPreviousEnabled = dayPreviousEnabled && (selectedDate >= displayDateStart.Value.Date);
            }
            if (dayPreviousButn != null)
                dayPreviousButn.IsEnabled = dayPreviousEnabled;

            if (selectedDate != null && displayDateEnd != null)
            {
                dayNextEnabled = dayNextEnabled && (selectedDate <= displayDateEnd.Value.Date);
            }
            if (dayNextButn != null)
                dayNextButn.IsEnabled = dayNextEnabled;
        }

        protected virtual string GetFormatSelectedDate()
        {
            var cultrueInfo = (this.Culture ?? Language.GetSpecificCulture());
            var formatInfo = cultrueInfo.DateTimeFormat;
            var dateFormat = this.SelectedDateFormat == DatePickerFormat.Long ? formatInfo.LongDatePattern : formatInfo.ShortDatePattern;

            var dateTimeFormat = string.Intern($"{dateFormat}");

            var selectedDate = this.SelectedDate;
            var formatedValue = selectedDate?.ToString(dateTimeFormat, cultrueInfo);
            return formatedValue;
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);
            // To hide the popup when the user e.g. alt+tabs, monitor for when the window becomes a background window.
            if (!(bool)e.NewValue)
            {
                this.IsDropDownOpen = false;
            }
        }

        private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs e)
        {
            this.IsDropDownOpen = false;
        }

        private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddp = (DayDatePicker)d;
            ddp.ApplySelectedDate();
        }

        private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddp = (DayDatePicker)d;
            ddp.ApplySelectedDate();
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddp = (DayDatePicker)d;
            ddp.OnSelectedDateChanged(new DayDatePickerSelectionChangedEventArgs(SelectedDateChangedEvent, (DateTime?)e.OldValue, (DateTime?)e.NewValue));
            ddp.ApplySelectedDate();
        }
        
        private static void OnSelectedDateFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddp = d as DayDatePicker;
            ddp.ApplySelectedDate();
        }

        private void OnCalendarSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var dt = (DateTime)e.AddedItems[0];

                var timeOfDay = SelectedDate.GetValueOrDefault().TimeOfDay;

                dt += timeOfDay;
                SelectedDate = dt;
            }
        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddp = d as DayDatePicker;
            ddp.ApplySelectedDate();
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddp = (DayDatePicker)d;

            if (e.NewValue is XmlLanguage)
            {
                ddp.Language = (XmlLanguage)e.NewValue;
            }
            else if (e.NewValue is System.Globalization.CultureInfo)
            {
                ddp.Language = XmlLanguage.GetLanguage(((System.Globalization.CultureInfo)e.NewValue).IetfLanguageTag);
            }
            else
            {
                ddp.Language = XmlLanguage.Empty;
            }

            ddp.ApplySelectedDate();
        }

        private void OnDayPreviousButtonClick(object sender, RoutedEventArgs e)
        {
            var selectDate = this.SelectedDate;
            if (selectDate != null)
            {
                this.SelectedDate = selectDate.Value.AddDays(-1);
            }
        }

        private void OnSelectedDayTitleButtonClick(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
            if (_calendarPopup != null)
            {
                _calendarPopup.IsOpen = IsDropDownOpen;
            }
        }

        private void OnDayNextButtonClick(object sender, RoutedEventArgs e)
        {
            var selectDate = this.SelectedDate;
            if (selectDate != null)
            {
                this.SelectedDate = selectDate.Value.AddDays(1);
            }
        }

    }
}
