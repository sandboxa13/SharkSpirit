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
        public SceneGraphViewModel(SceneGraphManager sceneGraphManager)
        {
            SelectedItem = new SceneGraphEntityViewModel(Entity.Empty(), sceneGraphManager);
            SceneGraphEntityViewModels = new ObservableCollection<SceneGraphEntityViewModel>();
            SceneCameras = new ObservableCollection<SceneGraphCameraViewModel>();



            AddEntityCommand = ReactiveCommand.Create(sceneGraphManager.AddEntity);

            CreateSceneViewModels(sceneGraphManager);

            SelectedCamera = SceneCameras.FirstOrDefault();

            this.WhenAnyValue(model => model.SelectedCamera)
                .Skip(1)
                .Subscribe(camera =>
            {
                if (camera == null)
                    return;

                sceneGraphManager.SelectCamera(camera.GetEntity());
            });


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
        }

        [Reactive] public ObservableCollection<SceneGraphCameraViewModel> SceneCameras { get; set; }
        [Reactive] public ObservableCollection<SceneGraphEntityViewModel> SceneGraphEntityViewModels { get; set; }
        [Reactive] public SceneGraphEntityViewModel SelectedItem { get; set; }
        [Reactive] public SceneGraphCameraViewModel SelectedCamera { get; set; }
        [Reactive] public ReactiveCommand<Unit, Unit> AddEntityCommand { get; set; }

        private void CreateSceneViewModels(SceneGraphManager sceneGraphManager)
        {
            sceneGraphManager.AddCamera();


            SceneGraphEntityViewModels.AddRange(
                sceneGraphManager
                    .GetSceneEntities()
                    .Select(entity => new SceneGraphEntityViewModel(entity, sceneGraphManager)));

            SceneCameras.AddRange(
                sceneGraphManager
                    .GetSceneCameras()
                    .Select(component => new SceneGraphCameraViewModel(component.Entity, sceneGraphManager)));

        }
    }
}
