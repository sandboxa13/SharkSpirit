using SharkSpirit.Core;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class TransformComponent : EntityComponent
    {
        public TransformComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Transform, "Transform component")
        {
        }
        
        public Matrix WorldMatrix => Matrix.RotationX(Rotation.X) * Matrix.RotationY(Rotation.Y) * Matrix.RotationZ(Rotation.Z) *
            Matrix.Translation(Position.X, Position.Y, Position.Z);

        public Vector3 Position;

        public Quaternion Rotation;

        public Vector3 Scale;
    }
}