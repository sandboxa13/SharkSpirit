using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.AvalonDock.Layout;

namespace SharkSpirit.Modules.Core.AvalonDock
{
    public class DockingManagerRegionAdapterLayoutStrategy : ILayoutUpdateStrategy
    {
        private readonly ILayoutAnchorableHelper _layoutAnchorableHelper;
        private readonly ILayoutUpdateStrategy _wrappedStrategy;

        public DockingManagerRegionAdapterLayoutStrategy(ILayoutUpdateStrategy wrappedStrategy, ILayoutAnchorableHelper layoutAnchorableHelper)
        {
            _layoutAnchorableHelper = layoutAnchorableHelper;
            _wrappedStrategy = wrappedStrategy ?? new EmptyDockingManagerRegionAdapterLayoutStrategy();
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            _wrappedStrategy.AfterInsertAnchorable(layout, anchorableShown);
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
            _wrappedStrategy.AfterInsertDocument(layout, anchorableShown);
        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            if (layout == null || anchorableToShow == null) return false;

            var result = false;
            var destPane = destinationContainer as LayoutAnchorablePane;
            if (anchorableToShow.Root == null)
            {
                var attr = GetAvalonDockAnchorableAttribute(anchorableToShow);

                anchorableToShow.AddToLayout(layout.Manager, GetContentAnchorableStrategy(attr.Strategy));

                if (attr.IsHidden)
                {
                    anchorableToShow.CanHide = true;
                    anchorableToShow.Hide();
                }

                anchorableToShow.Title = attr.Title;
                (anchorableToShow.Parent as LayoutAnchorablePane).DockWidth = new GridLength(attr.Size);
                _layoutAnchorableHelper.AddLayoutAnchorable(anchorableToShow);
                result = true;
            }
            else if (destPane != null && anchorableToShow.IsHidden)
            {
                // Show a hidden Anchorable.
                if (anchorableToShow.PreviousContainerIndex < 0)
                {
                    destPane.Children.Add(anchorableToShow);
                }
                else
                {
                    var insertIndex = anchorableToShow.PreviousContainerIndex;
                    if (insertIndex > destPane.ChildrenCount)
                    {
                        insertIndex = destPane.ChildrenCount;
                    }
                    destPane.Children.Insert(insertIndex, anchorableToShow);
                }
                result = true;
            }
            return result || _wrappedStrategy.BeforeInsertAnchorable(layout, anchorableToShow, destinationContainer);
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            // todo: переделать
            BindingOperations.SetBinding(
                anchorableToShow,
                LayoutDocument.TitleProperty,
                new Binding("Name")
                {
                    Source = (anchorableToShow.Content as FrameworkElement)?.DataContext,
                });

            return _wrappedStrategy.BeforeInsertDocument(layout, anchorableToShow, destinationContainer);
        }

        private static AvalonDockAnchorableAttribute GetAvalonDockAnchorableAttribute(LayoutAnchorable anchorable)
        {
            if (AvalonDockAnchorableAttributeHelper.IsAnchorable(anchorable.Content))
            {
                return AvalonDockAnchorableAttributeHelper.GetAvalonDockAnchorableAttribute(anchorable.Content);
            }

            throw new InvalidOperationException();
        }

        private static AnchorableShowStrategy GetContentAnchorableStrategy(AnchorableStrategy anchorableStrategy)
        {
            AnchorableShowStrategy flag = 0;
            foreach (var strategyFlag in SplitAnchorableStrategies(anchorableStrategy))
            {
                AnchorableShowStrategy strategy;
                switch (strategyFlag)
                {
                    case AnchorableStrategy.Most:
                        strategy = AnchorableShowStrategy.Most;
                        break;
                    case AnchorableStrategy.Left:
                        strategy = AnchorableShowStrategy.Left;
                        break;
                    case AnchorableStrategy.Right:
                        strategy = AnchorableShowStrategy.Right;
                        break;
                    case AnchorableStrategy.Top:
                        strategy = AnchorableShowStrategy.Top;
                        break;
                    case AnchorableStrategy.Bottom:
                        strategy = AnchorableShowStrategy.Bottom;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                flag |= strategy;
            }
            if (flag == 0)
            {
                flag = AnchorableShowStrategy.Most;
            }
            return flag;
        }


        private static AnchorableStrategy[] SplitAnchorableStrategies(AnchorableStrategy strategy)
        {
            return Enum
                .GetValues(typeof(AnchorableStrategy))
                .Cast<AnchorableStrategy>()
                .Where(value => strategy.HasFlag(value))
                .ToArray();
        }

        private class EmptyDockingManagerRegionAdapterLayoutStrategy : ILayoutUpdateStrategy
        {
            public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
            {
            }

            public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
            {
            }

            public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
            {
                return false;
            }

            public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
            {
                return false;
            }
        }
    }
}