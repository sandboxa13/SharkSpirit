using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine.Components;

namespace SharkSpirit.Modules.SceneInspector.ViewModels.Components
{
    public class TransformComponentViewModel : ComponentBaseViewModel
    {
        public TransformComponentViewModel(TransformComponent transformComponent) : base(transformComponent)
        {
            PositionX = Math.Round(transformComponent.Position.X, 2);
            PositionY = Math.Round(transformComponent.Position.Y, 2);
            PositionZ = Math.Round(transformComponent.Position.Z, 2);

            RotationX = transformComponent.Rotation.X;
            RotationY = transformComponent.Rotation.Y;
            RotationZ = transformComponent.Rotation.Z;

            InitializeSubscriptions(transformComponent);
        }

        [Reactive] public double PositionX { get; set; }
        [Reactive] public double PositionY { get; set; }
        [Reactive] public double PositionZ { get; set; }

        [Reactive] public double RotationX { get; set; }
        [Reactive] public double RotationY { get; set; }
        [Reactive] public double RotationZ { get; set; }
        
        private void InitializeSubscriptions(TransformComponent transformComponent)
        {
            this.WhenAnyValue(model => model.PositionX).Subscribe(value => { transformComponent.Position.X = (float) value; });
            this.WhenAnyValue(model => model.PositionY).Subscribe(value => { transformComponent.Position.Y = (float)value; });
            this.WhenAnyValue(model => model.PositionZ).Subscribe(value => { transformComponent.Position.Z = (float)value; });

            this.WhenAnyValue(model => model.RotationX).Subscribe(value =>
            {
                transformComponent.Rotation.X = (float) ((float)value * (Math.PI/180));
            });
            this.WhenAnyValue(model => model.RotationY).Subscribe(value =>
            {
                transformComponent.Rotation.Y = (float) ((float)value * (Math.PI/180));
            });
            this.WhenAnyValue(model => model.RotationZ).Subscribe(value =>
            {
                transformComponent.Rotation.Z = (float) ((float)value * (Math.PI/180));
            });
        }
    }
}