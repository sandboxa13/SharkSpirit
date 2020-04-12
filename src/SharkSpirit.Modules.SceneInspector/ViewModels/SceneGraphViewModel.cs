using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneGraphViewModel : ViewModelBase
    {
        public SceneGraphViewModel(SceneGraphManager sceneGraphManager)
        {
            SelectedItem = new SceneGraphEntityViewModel(Entity.Empty(), sceneGraphManager);
            SceneGraphEntityViewModels = new ObservableCollection<SceneGraphEntityViewModel>();

            CreateSceneViewModels(sceneGraphManager);

            this.WhenAnyValue(model => model.SelectedItem)
                .Skip(1)
                .Subscribe(item =>
                {
                    if (item == null)
                        return;

                    sceneGraphManager.ChangeSelectedItem(item.GetEntity());
                });

            sceneGraphManager.EntityRemovedObservable.Skip(1)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entity =>
            {
                SelectedItem = null;

                var vm = SceneGraphEntityViewModels.FirstOrDefault(model => model.Id == entity.Id);

                if(vm == null)
                    return;

                SceneGraphEntityViewModels.Remove(vm);
            });
        }

        [Reactive] public ObservableCollection<SceneGraphEntityViewModel> SceneGraphEntityViewModels { get; set; }
        [Reactive] public SceneGraphEntityViewModel SelectedItem { get; set; }

        private void CreateSceneViewModels(SceneGraphManager sceneGraphManager)
        {
            SceneGraphEntityViewModels.AddRange(
                sceneGraphManager
                    .GetSceneEntities()
                    .Select(entity => new SceneGraphEntityViewModel(entity, sceneGraphManager)));
        }
    }
}
