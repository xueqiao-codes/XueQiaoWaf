﻿using BolapanControl.ItemsFilter.Initializer;
using BolapanControl.ItemsFilter.Model;
using BolapanControl.ItemsFilter.View;
using BolapanControl.ItemsFilter.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BolapanControl.ItemsFilter {

    /// <summary>
    /// Visual states of FilterControl.
    /// </summary>
    [Flags]
    public enum FilterControlState
    {
        /// <summary>
        /// FilterControl is disable.
        /// </summary>
        Disable = 0,
        /// <summary>
        /// FilterControl is enable.
        /// </summary>
        Enable = 1,
        /// <summary>
        /// DropDown part is show.
        /// </summary>
        Open = 2,
        /// <summary>
        /// Filter is active.
        /// </summary>
        Active = 4,
        OpenActive = 6,
    }

    /// <summary>
    /// Common Filter control that represent collection of Filters in UI.
    /// </summary>
    [TemplateVisualState(GroupName = "FilterState", Name = "Disable")]
    [TemplateVisualState(GroupName = "FilterState", Name = "Enable")]
    [TemplateVisualState(GroupName = "FilterState", Name = "Active")]
    [TemplateVisualState(GroupName = "FilterState", Name = "Open")]
    [TemplateVisualState(GroupName = "FilterState", Name = "OpenActive")]
    public partial  class FilterControl : ItemsControl {
        
        private static bool isInDesignMode = (bool)System.ComponentModel.DependencyPropertyDescriptor.FromProperty(
                                System.ComponentModel.DesignerProperties.IsInDesignModeProperty,
                                typeof(System.Windows.DependencyObject)
                                ).Metadata.DefaultValue;
        static FilterControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterControl),
                new FrameworkPropertyMetadata(typeof(FilterControl)));
        }
        private bool isLoaded;
        /// <summary>
        /// Instance of FilterPresenter that attached to FilterControl.
        /// </summary>
        protected FilterPresenter filterPresenter;
        /// <summary>
        /// Create new instance of FilterControl.
        /// </summary>
        public FilterControl() {
            Loaded += FilterControl_Loaded;
            Unloaded += FilterControl_Unloaded;
        }

        #region dp object ParentCollection

        /// <summary>
        /// Parent Dependency Property
        /// </summary>
        public static readonly DependencyProperty ParentCollectionProperty =
            DependencyProperty.Register("ParentCollection", typeof(IEnumerable), typeof(FilterControl),
                new FrameworkPropertyMetadata((object)null,
                    new PropertyChangedCallback(OnParentCollectionChanged)));

        /// <summary>
        /// Gets or sets the source collection for filtering. 
        /// May be ICollectionView or any IEnumerable.
        /// If Parent is ICollectionView, FilterControl bind to Parent itself;
        /// otherwise, FilterControl bind to default collection view for the given source collection.
        /// </summary>
        public IEnumerable ParentCollection {
            get {
                return (IEnumerable)GetValue(ParentCollectionProperty);
            }
            set {
                SetValue(ParentCollectionProperty, value);
            }
        }

        /// <summary>
        /// Handles changes to the Parent property.
        /// </summary>
        private static void OnParentCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            FilterControl target = (FilterControl)d;
            IEnumerable oldParent = (IEnumerable)e.OldValue;
            IEnumerable newParent = (IEnumerable)e.NewValue;
            target.OnParentCollectionChanged(oldParent, newParent);
        }


        /// <summary>
        /// Trying to load a model and provides derived classes an opportunity to handle changes to the Parent property.
        /// </summary>
        protected virtual void OnParentCollectionChanged(IEnumerable oldParent, IEnumerable newParent) {
            if (isLoaded ) {
                LoadModel();
            }
        }

        #endregion
        #region Key

        /// <summary>
        /// Key Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(FilterControl),
                new FrameworkPropertyMetadata((string)null,
                    new PropertyChangedCallback(OnKeyChanged)));
        /// <summary>
        /// Gets or sets the key for the identification filter (the filter set) in the parent collection view filters.
        /// The key and initializers from FilterInitializersManager define a set of filters that ItemsPresenter provides into a view model for the  FilterControl.
        /// If multiple instances of the Filter Control have the same key and FilterInitializer in FilterInitializersManager, 
        /// then these instances  will display the same filter in the view model.
        /// Derived classes can give an additional meaning to the value of the properties Key. 
        /// For example, the class ColumnFilter uses key value as name of the item property.
        /// </summary>
        public string Key {
            get {
                return (string)GetValue(KeyProperty);
            }
            set {
                SetValue(KeyProperty, value);
            }
        }

        /// <summary>
        /// Handles changes to the Key property.
        /// </summary>
        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            FilterControl target = (FilterControl)d;
            if (!isInDesignMode && target.Model!=null )
                throw new InvalidOperationException("Change Key after FilterControl was attached to FilterPresenter not allowed.");
            string oldKey = (string)e.OldValue;
            string newKey = (string)e.NewValue;
            target.OnKeyChanged(oldKey, newKey);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Key property.
        /// </summary>
        protected virtual void OnKeyChanged(string oldKey, string newKey) {
          }

        #endregion
        #region FilterInitializersManager

        /// <summary>
        /// FilterInitializersManager Dependency Property
        /// </summary>
        public static readonly DependencyProperty FilterInitializersManagerProperty =
            DependencyProperty.Register("FilterInitializersManager", typeof(FilterInitializersManager), typeof(FilterControl),
                new FrameworkPropertyMetadata((FilterInitializersManager)null,
                    new PropertyChangedCallback(OnFilterInitializersManagerChanged)));
        /// <summary>
        /// Gets or sets the FilterInitializersManager.
        /// FilterInitializersManager contains a set of initializers determines the composition of the filters in the view model.
        /// If FIlterInitializersManager is null, it used default FilterInitialisersManager that provided by FilterInitializersManager.Default static property.
        /// FilterInitializersManager.Default included EqualFilterInitializer, LessOrEqualFilterInitializer, GreaterOrEqualFilterInitializer,
        ///                                         RangeFilterInitializer(), StringFilterInitializer(), EnumFilterInitializer().
        /// </summary>
        public FilterInitializersManager FilterInitializersManager {
            get {
                return (FilterInitializersManager)GetValue(FilterInitializersManagerProperty);
            }
            set {
                SetValue(FilterInitializersManagerProperty, value);
                
            }
        }

        /// <summary>
        /// Handles changes to the FilterInitializersManager property.
        /// </summary>
        private static void OnFilterInitializersManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            FilterControl target = (FilterControl)d;
            if (!isInDesignMode && target.Model!=null)
                throw new InvalidOperationException("Change Key after FilterControl was attached to FilterPresenter not allowed.");
            FilterInitializersManager oldFilterInitializersManager = (FilterInitializersManager)e.OldValue;
            FilterInitializersManager newFilterInitializersManager = (FilterInitializersManager)e.NewValue;
            target.OnFilterInitializersManagerChanged(oldFilterInitializersManager, newFilterInitializersManager);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FilterInitializersManager property.
        /// </summary>
        protected virtual void OnFilterInitializersManagerChanged(FilterInitializersManager oldFilterInitializersManager, FilterInitializersManager newFilterInitializersManager) {
        }

        #endregion
        #region Model

        /// <summary>
        /// Model Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey ModelPropertyKey
            = DependencyProperty.RegisterReadOnly("Model", typeof(FilterControlVm), typeof(FilterControl),
                new FrameworkPropertyMetadata((FilterControlVm)null,
                    new PropertyChangedCallback(OnModelChanged)));

        public static readonly DependencyProperty ModelProperty
            = ModelPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the FilterControlVm. 
        /// </summary>
        /// <return>Instance of FilterControlVm if FilterControl was successfully attached to ParentCollection, otherwise null.</return>
        public FilterControlVm Model {
            get { return (FilterControlVm)GetValue(ModelProperty); }
        }

        /// <summary>
        /// Handles changes to the Model property.
        /// </summary>
        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            FilterControl target = (FilterControl)d;

            FilterControlVm oldModel = (FilterControlVm)e.OldValue;
            if (oldModel != null) {
                oldModel.Dispose();
                oldModel.StateChanged -= target.OnStateChanged;
                target.ItemsSource = null;
            }
            FilterControlVm newModel = target.Model;
            if (newModel != null) {
                newModel.StateChanged += target.OnStateChanged;
                target.ItemsSource = newModel;
                target.SetVisualState(newModel.State);
            }
            target.OnModelChanged(newModel);
        }


        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Model property.
        /// </summary>
        protected virtual void OnModelChanged(FilterControlVm newModel) {
        }

        #endregion
         private void LoadModel() {
            SetValue(ModelPropertyKey, CreateModel());
        }
        /// <summary>
        /// Create model for FilterControl, if it is possible in current state.
        /// </summary>
        /// <returns>Instance of FilterControlVm or null.</returns>
       protected virtual FilterControlVm CreateModel() {
            FilterControlVm vm = null;
            filterPresenter = Parent == null ? null : FilterPresenter.TryGet(ParentCollection);
            if (filterPresenter != null) {
                vm = filterPresenter.TryGetFilterControlVm(Key, FilterInitializersManager);
            }
             return vm;
        }
       //
       // Summary:
       //     Prepares the specified element to display the specified item.
       //
       // Parameters:
       //   element:
       //     Element used to display the specified item.
       //
       //   item:
       //     Specified item.
        protected override void PrepareContainerForItemOverride(DependencyObject container, object item) {
            base.PrepareContainerForItemOverride(container, item);
            if (item != null && container is ContentPresenter) {

                if (((ContentPresenter)container).ContentTemplate != null) return;
                if (((ContentPresenter)container).ContentTemplateSelector != null)
                {
                    var selector = ((ContentPresenter)container).ContentTemplateSelector;
                    var contentTemplate = selector.SelectTemplate(item, container);
                    if (contentTemplate != null) return;
                }

                Type itemType = item.GetType();
                object[] viewAttr = itemType.GetCustomAttributes(typeof(ViewAttribute), true);
                if (viewAttr.Length > 0) {
                    UIElement view = null;
                    try {
                        Type viewType = ((ViewAttribute)viewAttr[0]).ViewType;
                        if (viewType.GetCustomAttributes(typeof(ModelViewAttribute), true).Length > 0)
                        // Ok, make new view using constructor ...FilterView(model).
                        {
                            if (viewType.IsGenericType) {
                                Type[] typeArgs = itemType.GetGenericArguments();
                                Type constructed = viewType.MakeGenericType(typeArgs);
                                view = (UIElement)Activator.CreateInstance(constructed, item);
                            }
                            else {
                                view = (UIElement)Activator.CreateInstance(viewType, item);
                            }
                        }
                    }
                    catch (Exception ex) {
                        int identLevel = Debug.IndentLevel;
                        do {
                            Debug.WriteLine(ex.Message);
                            ex = ex.InnerException;
                            Debug.Indent();
                        } while (ex!=null);
                        Debug.IndentLevel = identLevel;
                    }
                    ((ContentPresenter)container).Content = view;
                }
            }
        }

        private void FilterControl_Loaded(object sender, RoutedEventArgs e) {
            isLoaded = true;
            LoadModel();
        }

        private void FilterControl_Unloaded(object sender, RoutedEventArgs e) {
            SetValue(ModelPropertyKey, null);
            isLoaded = false;
        }

        private void OnStateChanged(FilterControlVm sender, FilterControlState filterState)
        {
            SetVisualState(filterState);
        }

        private void SetVisualState(FilterControlState filterState) {
            var oldState = this.FilterControlState;
            var newState = filterState;

            this.FilterControlState = newState;
            FilterControlStateChangedCallbak?.Invoke(new FilterControlStateChangedArgs(this, oldState, newState));
            
            if (filterState == FilterControlState.Disable)
                VisualStateManager.GoToState(this, filterState.ToString(), false);
            else if ((filterState & FilterControlState.OpenActive) == FilterControlState.OpenActive)
                VisualStateManager.GoToState(this, FilterControlState.OpenActive.ToString(), false);
            else if (filterState.HasFlag(FilterControlState.Open))
                VisualStateManager.GoToState(this, FilterControlState.Open.ToString(), false);
            else if (filterState.HasFlag(FilterControlState.Active))
                VisualStateManager.GoToState(this, FilterControlState.Active.ToString(), false);
            else if (filterState.HasFlag(FilterControlState.Enable))
                VisualStateManager.GoToState(this, FilterControlState.Enable.ToString(), false);
            else
                VisualStateManager.GoToState(this, filterState.ToString(), true);
        }


        #region FilterControlState

        public FilterControlState FilterControlState
        {
            get { return (FilterControlState)GetValue(FilterControlStateProperty); }
            private set { SetValue(FilterControlStatePropertyKey, value); }
        }

        public static readonly DependencyPropertyKey FilterControlStatePropertyKey =
            DependencyProperty.RegisterReadOnly("FilterControlState", typeof(FilterControlState), typeof(FilterControl), 
                new PropertyMetadata(FilterControlState.Disable, OnFilterControlStateChanged));

        public static readonly DependencyProperty FilterControlStateProperty =
            FilterControlStatePropertyKey.DependencyProperty;

        public static readonly RoutedEvent FilterControlStateChangedEvent =
           EventManager.RegisterRoutedEvent(
               "FilterControlStateChanged",
               RoutingStrategy.Bubble,
               typeof(RoutedPropertyChangedEventHandler<FilterControlState>),
               typeof(FilterControl));

        public event RoutedPropertyChangedEventHandler<FilterControlState> FilterControlStateChanged
        {
            add { AddHandler(FilterControlStateChangedEvent, value); }
            remove { RemoveHandler(FilterControlStateChangedEvent, value); }
        }

        private static void OnFilterControlStateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (FilterControl)d;
            var args = new RoutedPropertyChangedEventArgs<FilterControlState>(
                (FilterControlState)e.OldValue,
                (FilterControlState)e.NewValue)
            { RoutedEvent = FilterControlStateChangedEvent };
            instance.RaiseEvent(args);
        }

        #endregion


        #region FilterControlStateChangedCallbak

        public FilterControlStateChanged FilterControlStateChangedCallbak
        {
            get { return (FilterControlStateChanged)GetValue(FilterControlStateChangedCallbakProperty); }
            set { SetValue(FilterControlStateChangedCallbakProperty, value); }
        }

        public static readonly DependencyProperty FilterControlStateChangedCallbakProperty =
            DependencyProperty.Register("FilterControlStateChangedCallbak", typeof(FilterControlStateChanged), typeof(FilterControl), new PropertyMetadata(null));
        
        #endregion

    }
}
