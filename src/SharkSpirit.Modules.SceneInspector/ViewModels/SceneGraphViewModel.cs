using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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
        private readonly SceneGraphManager _sceneGraphManager;

        public SceneGraphViewModel(SceneGraphManager sceneGraphManager)
        {
            _sceneGraphManager = sceneGraphManager;
            SelectedItem = new SceneGraphEntityViewModel(Entity.Empty(sceneGraphManager.Container), sceneGraphManager);
            SceneGraphEntityViewModels = new ObservableCollection<SceneGraphEntityViewModel>();

            AddEntityCommand = ReactiveCommand.Create(sceneGraphManager.AddEntity);

            CreateSceneViewModels(sceneGraphManager);

            this.WhenAnyValue(model => model.SelectedItem)
                .Skip(1)
                .Subscribe(item =>
                {
                    if (item == null)
                        return;

                    sceneGraphManager.ChangeSelectedItem(item.GetEntity());
                });

            sceneGraphManager.EntityRemovedObservable
                .Skip(1)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entity =>
            {
                SelectedItem = null;

                var vm = SceneGraphEntityViewModels.FirstOrDefault(model => model.Id == entity.Id);

                if(vm == null)
                    return;

                SceneGraphEntityViewModels.Remove(vm);
            });

            sceneGraphManager.EntityAddedObservable
                .Skip(1)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entity =>
                {
                    SceneGraphEntityViewModels.Add(new SceneGraphEntityViewModel(entity, sceneGraphManager));
                });

            sceneGraphManager.Container.GetService<Game>().GameUpdated += (sender, args) =>
            {
                Refresh();
            };
        }

       

        [Reactive] public ObservableCollection<SceneGraphEntityViewModel> SceneGraphEntityViewModels { get; set; }
        [Reactive] public SceneGraphEntityViewModel SelectedItem { get; set; }
        [Reactive] public ReactiveCommand<Unit, Unit> AddEntityCommand { get; set; }

        private void CreateSceneViewModels(SceneGraphManager sceneGraphManager)
        {
            sceneGraphManager.AddCamera();

            foreach (var entity in sceneGraphManager.GetSceneEntities())
            {
                SceneGraphEntityViewModels.Add(BuildTree(entity, new SceneGraphCameraViewModel(entity, _sceneGraphManager)));
            }

            SceneGraphEntityViewModels.AddRange(
                sceneGraphManager
                    .GetSceneCameras()
                    .Select(component => new SceneGraphCameraViewModel(component.Entity, sceneGraphManager)));

        }

        private SceneGraphEntityViewModel BuildTree(Entity entity, SceneGraphEntityViewModel parentVm)
        {
            foreach (var entityChild in entity.Childs)
            {
                var sceneGraphEntityViewModel = new SceneGraphEntityViewModel(entityChild, _sceneGraphManager);

                parentVm.Childs.Add(new SceneGraphCameraViewModel(entityChild, _sceneGraphManager));
                BuildTree(entityChild, sceneGraphEntityViewModel);
            }
            
            return parentVm;
        }

        public void Refresh()
        {
            foreach (var sceneGraphEntityViewModel in SceneGraphEntityViewModels)
            {
                sceneGraphEntityViewModel.Refresh();
            }
            
            SelectedItem.Refresh();
        }
    }
}
