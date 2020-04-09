using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Prism.Events;
using Prism.Regions;
using Prism.Regions.Behaviors;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;


namespace SharkSpirit.Modules.Core.AvalonDock
{
    public class DockingManagerLayoutContentSyncBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
        public static readonly string BehaviorKey = "DockingManagerLayoutContentSyncBehavior";

        private bool _updatingActiveViewsInManagerActiveContentChanged;
        private DockingManager _dockingManager;
        private readonly IEventAggregator _eventAggregator;

        private readonly ObservableCollection<object> _documents = new ObservableCollection<object>();
        private ReadOnlyObservableCollection<object> _readonlyDocumentsList;

        private readonly ObservableCollection<object> _anchorables = new ObservableCollection<object>();
        private ReadOnlyObservableCollection<object> _readonlyAnchorablesList;

        public DockingManagerLayoutContentSyncBehavior(
            DockingManager dockingManager,
            IEventAggregator eventAggregator)
        {
            _dockingManager = dockingManager ?? throw new ArgumentNullException(nameof(dockingManager));
            _eventAggregator = eventAggregator;
        }

        public DependencyObject HostControl
        {
            get => _dockingManager;
            set => _dockingManager = value as DockingManager;
        }

        public ReadOnlyObservableCollection<object> Documents => _readonlyDocumentsList ??
                                                                 (_readonlyDocumentsList = new ReadOnlyObservableCollection<object>(_documents));

        public ReadOnlyObservableCollection<object> Anchorables => _readonlyAnchorablesList ??
                                                                   (_readonlyAnchorablesList = new ReadOnlyObservableCollection<object>(_anchorables));

        /// <summary>
        /// Starts to monitor the <see cref="IRegion"/> to keep it in synch with the items of the <see cref="HostControl"/>.
        /// </summary>
        protected override void OnAttach()
        {
            if (_dockingManager == null) throw new InvalidOperationException("DockingManager is not defined");
            if (_dockingManager.DocumentsSource != null) throw new InvalidOperationException("DocumentsSource must be null");
            if (_dockingManager.AnchorablesSource != null) throw new InvalidOperationException("AnchorablesSource must be null");
            if (BindingOperations.GetBinding(_dockingManager, DockingManager.DocumentsSourceProperty) != null) throw new InvalidOperationException("DocumentsSource must not be bound to anything");
            if (BindingOperations.GetBinding(_dockingManager, DockingManager.AnchorablesSourceProperty) != null) throw new InvalidOperationException("AnchorablesSource must not be bound to anything");

            SynchronizeItems();

            _dockingManager.ActiveContentChanged += DockingManagerActiveContentChangedHandler;
            Region.ActiveViews.CollectionChanged += RegionActiveViewsCollectionChangedHandler;
            Region.Views.CollectionChanged += RegionViewsCollectionChangedHandler;
            _dockingManager.DocumentClosed += DocumentClosedHandler;

            _windows = new List<LayoutFloatingWindowControl>();


            _dockingManager.LayoutUpdated += _dockingManager_LayoutChanged;
        }

        private List<LayoutFloatingWindowControl> _windows;

        private void _dockingManager_LayoutChanged(object sender, EventArgs e)
        {
            foreach (var floatingWindow in _dockingManager.FloatingWindows)
            {
                if (_windows.Contains((LayoutFloatingWindowControl) floatingWindow))
                    return;

                floatingWindow.Closing += FloatingWindow_Closing;
                floatingWindow.Closed += FloatingWindow_Closed;
                _windows.Add(floatingWindow);
            }
        }

        private void FloatingWindow_Closed(object sender, EventArgs e)
        {
            //var document = ((sender as LayoutAnchorableFloatingWindowControl).Model as LayoutAnchorableFloatingWindow)
            //    .RootPanel;

            //if (document == null) return;

            //if (Region.Views.Contains(document.Content))
            //{
            //    Region.Remove(document.Content);
            //}
        }

        private void FloatingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var window = (sender as LayoutFloatingWindowControl);

            _windows.Remove(window);
        }


        /// <summary>
        /// Binds the Documents Source and Anchorables Source of the Docking Manager
        /// and synchronizes them with the existing contents of the Region.
        /// </summary>
        private void SynchronizeItems()
        {
            BindingOperations.SetBinding(
                HostControl,
                DockingManager.DocumentsSourceProperty,
                new Binding(nameof(Documents))
                {
                    Source = this
                });
            BindingOperations.SetBinding(
               HostControl,
               DockingManager.AnchorablesSourceProperty,
               new Binding(nameof(Anchorables))
               {
                   Source = this
               });

            foreach (object view in Region.Views)
            {
                if (AvalonDockAnchorableAttributeHelper.IsAnchorable(view))
                {
                    _anchorables.Add(view);
                }
                else
                {
                    _documents.Add(view);
                }
            }
        }

        /// <summary>
        /// Activates the appropriate Views in the Region whenever the Active Content
        /// changes in the Docking Manager.
        /// </summary>
        private void DockingManagerActiveContentChangedHandler(object sender, EventArgs e)
        {
            try
            {
                _updatingActiveViewsInManagerActiveContentChanged = true;
                if (Equals(_dockingManager, sender))
                {
                    var activeContent = _dockingManager.ActiveContent;
                    foreach (var item in Region.ActiveViews.Where(it => it != activeContent))
                    {
                        if (item != null
                           && Region.Views.Contains(item)
                           && Region.ActiveViews.Contains(item))
                        {
                            Region.Deactivate(item);
                        }
                    }

                    if (activeContent != null
                       && Region.Views.Contains(activeContent)
                       && !Region.ActiveViews.Contains(activeContent))
                    {
                        Region.Activate(activeContent);
                    }
                }
            }
            finally
            {
                _updatingActiveViewsInManagerActiveContentChanged = false;
            }
        }

        /// <summary>
        /// Updates the Active Content of the Docking Manager whenever the Active Views
        /// change in the Region.
        /// </summary>
        private void RegionActiveViewsCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_updatingActiveViewsInManagerActiveContentChanged
               || e.NewItems == null)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (_dockingManager.ActiveContent != null
                   && _dockingManager.ActiveContent != e.NewItems[0]
                   && Region.ActiveViews.Contains(_dockingManager.ActiveContent))
                {
                    Region.Deactivate(_dockingManager.ActiveContent);
                }
                _dockingManager.ActiveContent = e.NewItems[0];
            }
            else
            {
                if (e.Action != NotifyCollectionChangedAction.Remove
                   || !e.OldItems.Contains(_dockingManager.ActiveContent))
                {
                    return;
                }
                _dockingManager.ActiveContent = null;
            }
        }

        /// <summary>
        /// Updates the collections bound to the Documents Source and Anchorables Source
        /// of the Docking Manager whenever the Views change in the Region.
        /// </summary>
        private void RegionViewsCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object newItem in e.NewItems)
                {
                    if (AvalonDockAnchorableAttributeHelper.IsAnchorable(newItem))
                    {
                        _anchorables.Add(newItem);
                    }
                    else
                    {
                        _documents.Add(newItem);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object oldItem in e.OldItems)
                {
                    if (AvalonDockAnchorableAttributeHelper.IsAnchorable(oldItem))
                    {
                        _anchorables.Remove(oldItem);
                    }
                    else
                    {
                        _documents.Remove(oldItem);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the Views collection in the Region whenever a Document is closed.
        /// </summary>
        private void DocumentClosedHandler(object sender, DocumentClosedEventArgs e)
        {
            var layoutDocument = e.Document;
            if (layoutDocument == null) return;

            if (Region.Views.Contains(layoutDocument.Content))
            {
                Region.Remove(layoutDocument.Content);
            }
        }
    }
}