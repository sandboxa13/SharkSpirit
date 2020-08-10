using DryIoc;
using Prism.Regions;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
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
            _container.Resolve<SharkSpirit.Core.IContainer>().AddService(_container.Resolve<Game>());

            var engineContainer = _container.Resolve<SharkSpirit.Core.IContainer>();
            
            _sceneGraphManager = new SceneGraphManager(engineContainer);

            SceneGraphViewModel = new SceneGraphViewModel(_sceneGraphManager);
            SceneItemInspectorViewModel = new SceneItemInspectorViewModel(_sceneGraphManager);

            _sceneGraphManager.Container.GetService<Game>().GameUpdated += (sender, args) =>
            {
                Refresh();
            };
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        
        private void Refresh()
        {
            SceneItemInspectorViewModel.Refresh();
        }
    }
}
