using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Engine.Components;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneItemInspectorViewModel : ViewModelBase
    {
        public SceneItemInspectorViewModel(SceneGraphManager sceneGraphManager)
        {
            sceneGraphManager.SelectedEntityChanged.Subscribe(UpdateSelectedItem);
        }

        private void UpdateSelectedItem(Entity entity)
        {
            SelectedSceneItemViewModel = new SelectedSceneItemViewModel(entity);
        }

        [Reactive] public SelectedSceneItemViewModel SelectedSceneItemViewModel { get; private set; }
    }

    public class SelectedSceneItemViewModel : ViewModelBase
    {
        public SelectedSceneItemViewModel(Entity entity)
        {
            Name = entity.Name;
            Id = entity.Id;

            TransformComponentViewModel = new TransformComponentViewModel(entity.TransformComponent);
        }

        [Reactive] public Guid Id { get; private set; }

        [Reactive] public TransformComponentViewModel TransformComponentViewModel { get; private set; }
    }

    public class TransformComponentViewModel :ViewModelBase
    {
        public TransformComponentViewModel(TransformComponent transformComponent)
        {
            PositionX = transformComponent.Position.X;
            PositionY = transformComponent.Position.Y;
            PositionZ = transformComponent.Position.Z;


            RotationX = transformComponent.Rotation.X;
            RotationY = transformComponent.Rotation.Y;
            RotationZ = transformComponent.Rotation.Z;

            this.WhenAnyValue(model => model.PositionX).Subscribe(value => { transformComponent.Position.X = (float) value; });
            this.WhenAnyValue(model => model.PositionY).Subscribe(value => { transformComponent.Position.Y = (float)value; });
            this.WhenAnyValue(model => model.PositionZ).Subscribe(value => { transformComponent.Position.Z = (float)value; });

            this.WhenAnyValue(model => model.RotationX).Subscribe(value => { transformComponent.Rotation.X = (float)value; });
            this.WhenAnyValue(model => model.RotationY).Subscribe(value => { transformComponent.Rotation.Y = (float)value; });
            this.WhenAnyValue(model => model.RotationZ).Subscribe(value => { transformComponent.Rotation.Z = (float)value; });
        }

        [Reactive] public double PositionX { get; set; }
        [Reactive] public double PositionY { get; set; }
        [Reactive] public double PositionZ { get; set; }

        [Reactive] public double RotationX { get; set; }
        [Reactive] public double RotationY { get; set; }
        [Reactive] public double RotationZ { get; set; }
    }
}
