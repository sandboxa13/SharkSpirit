using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneGraphViewModel : ViewModelBase
    {
        public SceneGraphViewModel(Logic.SceneGraphManager sceneGraphManager)
        {
            SelectedItem = new SceneGraphEntityViewModel(Entity.Empty());
            SceneGraphEntityViewModels = new ObservableCollection<SceneGraphEntityViewModel>();

            CreateSceneViewModels(sceneGraphManager);

            this.WhenAnyValue(model => model.SelectedItem)
                .Skip(1)
                .Subscribe(item =>
                {
                    sceneGraphManager.ChangeSelectedItem(item.GetEntity());
                });
        }

        [Reactive] public ObservableCollection<SceneGraphEntityViewModel> SceneGraphEntityViewModels { get; set; }
        [Reactive] public SceneGraphEntityViewModel SelectedItem { get; set; }

        private void CreateSceneViewModels(SceneGraphManager sceneGraphManager)
        {
            SceneGraphEntityViewModels.AddRange(
                sceneGraphManager
                    .GetSceneEntities()
                    .Select(entity => new SceneGraphEntityViewModel(entity)));
        }
    }
}
