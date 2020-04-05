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
            var position = new Vector3(Entity.TransformComponent.Position.X, Entity.TransformComponent.Position.Y, Entity.TransformComponent.Position.Z);

            var lookAt = new Vector3(0, 0, 1);

            var pitch = Entity.TransformComponent.Position.X * 0.0174532925f;
            var yaw = Entity.TransformComponent.Position.Y * 0.0174532925f; 
            var roll = Entity.TransformComponent.Position.Z * 0.0174532925f; 

            var rotationMatrix = Matrix.RotationYawPitchRoll(yaw, pitch, roll);

            lookAt = Vector3.TransformCoordinate(lookAt, rotationMatrix);
            var up = Vector3.TransformCoordinate(Vector3.UnitY, rotationMatrix);

            lookAt = position + lookAt;

            ViewMatrix = Matrix.LookAtLH(position, lookAt, up);
        }

       
    }
}