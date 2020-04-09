using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using Prism.Regions;

namespace SharkSpirit.Modules.Core.AvalonDock
{
    public sealed class DisposeClosedViewsBehavior : RegionBehavior
    {
        public const string BehaviorKey = "DisposableRegion";

        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += OnActiveViewsChanged;
        }

        private static void OnActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                DisposeViewsOrViewModels(e.OldItems);
            }
        }

        private static void DisposeViewsOrViewModels(IList views)
        {
            foreach (var view in views)
            {
                (view as IDisposable)?.Dispose();
                ((view as FrameworkElement)?.DataContext as IDisposable)?.Dispose();
            }
        }
    }
}
