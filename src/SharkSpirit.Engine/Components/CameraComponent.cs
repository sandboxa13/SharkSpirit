using System;
using SharkSpirit.Core;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class CameraComponent : EntityComponent
    {
        public CameraComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Camera)
        {
        }
        
        public Matrix ViewMatrix;
        
        public bool IsSelected { get; private set; }
        public void Update()
        {
            var position = new Vector3(Entity.TransformComponent.Position.X, Entity.TransformComponent.Position.Y,
                Entity.TransformComponent.Position.Z)
            {
                X = (float) (Entity.TransformComponent.Position.Z * Math.Sin(Entity.TransformComponent.Position.Y) *
                             Math.Cos(Entity.TransformComponent.Position.X)),
                Z = (float) (Entity.TransformComponent.Position.Z * Math.Sin(Entity.TransformComponent.Position.Y) *
                             Math.Sin(Entity.TransformComponent.Position.X)),
                Y = (float) (Entity.TransformComponent.Position.Z * Math.Cos(Entity.TransformComponent.Position.Y))
            };


            var lookAt = Vector3.Zero;

            var up = new Vector3(0, 1, 0);

            ViewMatrix = Matrix.LookAtLH(position, lookAt, up);
        }

        public void Select() => IsSelected = true;
        public void UnSelect() => IsSelected = false;
    }
}