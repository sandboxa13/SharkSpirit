using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneGraphEntityViewModel : ViewModelBase
    {
        private readonly Entity _entity;

        public SceneGraphEntityViewModel(Entity entity, SceneGraphManager sceneGraphManager)
        {
            _entity = entity;
            Name = entity.Name;
            Id = entity.Id;
            IsVisible = entity.IsVisible;
            Childs = new ObservableCollection<SceneGraphEntityViewModel>();
            
            RemoveEntityCommand = ReactiveCommand.Create(() =>
            {
                sceneGraphManager.RemoveEntity(entity);
            });

            this.WhenAnyValue(model => model.IsVisible)
                .Subscribe(b =>
                {
                    entity.ChangeIsVisible(b);
                    foreach (var sceneGraphEntityViewModel in Childs)
                    {
                        sceneGraphEntityViewModel.IsVisible = b;
                    }
                });
        }

        [Reactive] public ReactiveCommand<Unit, Unit> RemoveEntityCommand { get; set; }

        [Reactive] public ObservableCollection<SceneGraphEntityViewModel> Childs { get; set; }
        public Guid Id { get; private set; }

        [Reactive] public bool IsVisible { get; set; }

        public Entity GetEntity() => _entity;
    }


    public class SceneGraphCameraViewModel : SceneGraphEntityViewModel
    {
        public SceneGraphCameraViewModel(Entity entity, SceneGraphManager sceneGraphManager) : base(entity, sceneGraphManager)
        {
            SelectCameraCommand = ReactiveCommand.Create(() => sceneGraphManager.SelectCamera(entity));
        }

        [Reactive] public ReactiveCommand<Unit, Unit> SelectCameraCommand { get; set; }
    }
}