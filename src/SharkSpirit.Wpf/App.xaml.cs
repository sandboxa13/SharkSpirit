using System.Windows;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SharkSpirit.Modules.Core.Application;
using SharkSpirit.Modules.Core.AvalonDock;
using SharkSpirit.Modules.Core.Prism;
using Xceed.Wpf.AvalonDock;

namespace SharkSpirit.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RunLauncher();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            containerRegistry.Register<ILayoutAnchorableHelper, LayoutAnchorableHelper>();
            containerRegistry.Register<MainWindow>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping(typeof(DockingManager), Container.Resolve<DockingManagerRegionAdapter>());
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);

            regionBehaviors.AddIfMissing(nameof(DisposeClosedViewsBehavior), typeof(DisposeClosedViewsBehavior));
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog { ModulePath = @".\" };
        }

        private void RunLauncher()
        {
            Container.Resolve<IEventAggregator>()
                .GetEvent<ApplicationInitializedEvent>()
                .Subscribe(LauncherInitializedHandler, ThreadOption.PublisherThread);

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.SceneInspectorRegion, "SceneInspector");
            regionManager.RequestNavigate(RegionNames.SceneRegion, "Scene");

            //var launcherWindow = new MainWindow();
            //var launcherViewModel = Container.Resolve<MainWindowViewModel>();
            //launcherWindow.DataContext = launcherViewModel;
            //launcherWindow.Show();

            //launcherViewModel.InitAsync();
        }

        private void LauncherInitializedHandler(ApplicationInitializationResult result)
        {
            Container.Resolve<IEventAggregator>()
                .GetEvent<ApplicationInitializedEvent>()
                .Unsubscribe(LauncherInitializedHandler);

            //Current.Windows.OfType<LauncherWindow>().FirstOrDefault()?.Close();

            //switch (result)
            //{
            //    case ApplicationInitializationResult.Success:

            //        Container.Resolve<IEventAggregator>()
            //            .GetEvent<ApplicationInitializedEvent>()
            //            .Subscribe(MainWindowInitializeEvent, ThreadOption.PublisherThread);

            //        var regionManager = Container.Resolve<IRegionManager>();
            //        regionManager.RequestNavigate(RegionNames.SceneRegion, "Scene");
            //        break;
            //    case ApplicationInitializationResult.Failed:
            //        Current.Shutdown();
            //        break;
            //    case ApplicationInitializationResult.NeedRestart:
            //        Current.Shutdown();
            //        break;
            //    case ApplicationInitializationResult.NeedClose:
            //        Current.Shutdown();
            //        break;
            //}
        }

        private void MainWindowInitializeEvent(ApplicationInitializationResult result)
        {
            //Container.Resolve<IEventAggregator>()
            //    .GetEvent<ApplicationInitializedEvent>()
            //    .Unsubscribe(MainWindowInitializeEvent);

            //switch (result)
            //{
            //    case ApplicationInitializationResult.Success:

            //        break;
            //    case ApplicationInitializationResult.Failed:
            //    case ApplicationInitializationResult.NeedClose:
            //        Environment.Exit(0);
            //        break;
            //}
        }

    }
}