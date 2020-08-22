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
            var lookVector = Vector3.Transform(Vector3.UnitZ, Matrix.RotationYawPitchRoll(Entity.TransformComponent.Rotation.Y, Entity.TransformComponent.Rotation.Z, 0.0f));

            var camTarget = Entity.TransformComponent.Position + (Vector3) lookVector;

            ViewMatrix = Matrix.LookAtLH(Entity.TransformComponent.Position, camTarget, Vector3.UnitY);
        }

        public void Select() => IsSelected = true;
        public void UnSelect() => IsSelected = false;
    }
}