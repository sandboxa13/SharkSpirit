using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SharkSpirit.Modules.Core.AvalonDock;
using SharkSpirit.Modules.Core.Prism;
using SharkSpirit.Wpf.ViewModels.WorkSpaces.Main;
using Xceed.Wpf.AvalonDock;

namespace SharkSpirit.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        public override void Initialize()
        {
            Dispatcher.Thread.CurrentUICulture = new System.Globalization.CultureInfo("ru");

            base.Initialize();

            RunApp();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            var bootstrapper = new Bootstrapper();

            bootstrapper.RegisterTypes(containerRegistry.GetContainer());
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
            var window = Container.Resolve<MainWindow>();

            window.DataContext = new MainWindowViewModel();

            return window;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog { ModulePath = @".\" };
        }

        private void RunApp()
        {
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RequestNavigate(RegionNames.SceneRegion, "Scene");
            regionManager.RequestNavigate(RegionNames.SceneInspectorRegion, "SceneInspector");
        }
    }
}