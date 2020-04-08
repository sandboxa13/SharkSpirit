using System;
using DryIoc;
using Prism.Events;
using Prism.Regions;
using SharkSpirit.Modules.Core.AvalonDock;
using Xceed.Wpf.AvalonDock;

namespace LiveCenter.Prism
{
    internal class DockingManagerRegionAdapter : RegionAdapterBase<DockingManager>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainer _dryIocContainer;

        public DockingManagerRegionAdapter(
            IRegionBehaviorFactory regionBehaviorFactory,
            IEventAggregator eventAggregator,
            IContainer dryIocContainer) : base(regionBehaviorFactory)
        {
            _eventAggregator = eventAggregator;
            _dryIocContainer = dryIocContainer;
        }

        protected override void Adapt(IRegion region, DockingManager regionTarget)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            if (regionTarget == null) throw new ArgumentNullException(nameof(regionTarget));

            var currentLayoutStrategy = regionTarget.LayoutUpdateStrategy;
            regionTarget.LayoutUpdateStrategy = new DockingManagerRegionAdapterLayoutStrategy(currentLayoutStrategy, _dryIocContainer.Resolve<ILayoutAnchorableHelper>());

            // Add the behavior that synchronizes the items source items with the rest of the items.
            var regionBehavior = new DockingManagerLayoutContentSyncBehavior(
                dockingManager: regionTarget,
                eventAggregator: _eventAggregator);

            region.Behaviors.Add(
                key: DockingManagerLayoutContentSyncBehavior.BehaviorKey,
                regionBehavior: regionBehavior);

            AttachBehaviors(region, regionTarget);
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}