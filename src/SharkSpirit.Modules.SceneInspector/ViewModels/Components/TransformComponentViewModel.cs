using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine.Components;

namespace SharkSpirit.Modules.SceneInspector.ViewModels.Components
{
    public class TransformComponentViewModel : ComponentBaseViewModel
    {
        private readonly TransformComponent _transformComponent;

        public TransformComponentViewModel(TransformComponent transformComponent) : base(transformComponent)
        {
            _transformComponent = transformComponent;
            PositionX = Math.Round(transformComponent.Position.X, 2);
            PositionY = Math.Round(transformComponent.Position.Y, 2);
            PositionZ = Math.Round(transformComponent.Position.Z, 2);

            RotationX = transformComponent.Rotation.X;
            RotationY = transformComponent.Rotation.Y;
            RotationZ = transformComponent.Rotation.Z;

            InitializeSubscriptions();
        }

        [Reactive] public double PositionX { get; set; }
        [Reactive] public double PositionY { get; set; }
        [Reactive] public double PositionZ { get; set; }

        [Reactive] public double RotationX { get; set; }
        [Reactive] public double RotationY { get; set; }
        [Reactive] public double RotationZ { get; set; }

        public override void Refresh()
        {
            PositionX = Math.Round(_transformComponent.Position.X, 2);
            PositionY = Math.Round(_transformComponent.Position.Y, 2);
            PositionZ = Math.Round(_transformComponent.Position.Z, 2);

            RotationX = _transformComponent.Rotation.X;
            RotationY = _transformComponent.Rotation.Y;
            RotationZ = _transformComponent.Rotation.Z;
        }

        private void InitializeSubscriptions()
        {
            this.WhenAnyValue(model => model.PositionX)
                .Subscribe(value => { _transformComponent.Position.X = (float) value; });
            
            this.WhenAnyValue(model => model.PositionY)
                .Subscribe(value => { _transformComponent.Position.Y = (float)value; });
            
            this.WhenAnyValue(model => model.PositionZ)
                .Subscribe(value => { _transformComponent.Position.Z = (float)value; });

            this.WhenAnyValue(model => model.RotationX)
                .Subscribe(value => { _transformComponent.Rotation.X = (float) ((float)value * (Math.PI/180)); });
            
            this.WhenAnyValue(model => model.RotationY)
                .Subscribe(value => { _transformComponent.Rotation.Y = (float) ((float)value * (Math.PI/180)); });
            
            this.WhenAnyValue(model => model.RotationZ)
                .Subscribe(value => { _transformComponent.Rotation.Z = (float) ((float)value * (Math.PI/180)); });
        }
    }
}