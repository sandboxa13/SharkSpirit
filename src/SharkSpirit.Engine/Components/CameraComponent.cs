using System;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class CameraComponent : EntityComponent
    {
        public CameraComponent(Entity entity) : base(entity)
        {
        }
        
        public Matrix ViewMatrix;
        
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
    }
}