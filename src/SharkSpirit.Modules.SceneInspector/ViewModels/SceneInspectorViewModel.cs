using System.Collections.ObjectModel;
using System.Linq;
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

        public SceneInspectorViewModel(IContainer container)
        {
            _container = container;
        }

        [Reactive] public ObservableCollection<SceneGraphEntityViewModel> SceneGraphEntityViewModels { get; set; }

        private void CreateSceneViewModels(SceneGraphManager sceneGraphManager)
        {
            SceneGraphEntityViewModels.AddRange(
                sceneGraphManager
                    .GetSceneEntities()
                    .Select(entity => new SceneGraphEntityViewModel(entity)));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SceneGraphEntityViewModels = new ObservableCollection<SceneGraphEntityViewModel>();

            var engineContainer = _container.Resolve<SharkSpirit.Core.IContainer>();

            var sceneGraphManager = new SceneGraphManager(engineContainer);

            CreateSceneViewModels(sceneGraphManager);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
