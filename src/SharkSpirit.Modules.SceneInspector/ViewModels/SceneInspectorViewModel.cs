using DryIoc;
using Prism.Regions;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneInspectorViewModel : DockWindowViewModel, INavigationAware
    {
        private readonly IContainer _container;
        private SceneGraphManager _sceneGraphManager;
        public SceneInspectorViewModel(IContainer container)
        {
            _container = container;
        }

        [Reactive] public SceneGraphViewModel SceneGraphViewModel { get; private set; }
        [Reactive] public SceneItemInspectorViewModel SceneItemInspectorViewModel { get; private set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var engineContainer = _container.Resolve<SharkSpirit.Core.IContainer>();

            _sceneGraphManager = new SceneGraphManager(engineContainer);

            SceneGraphViewModel = new SceneGraphViewModel(_sceneGraphManager);
            SceneItemInspectorViewModel = new SceneItemInspectorViewModel(_sceneGraphManager);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
