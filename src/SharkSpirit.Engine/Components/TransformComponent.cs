using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class TransformComponent : EntityComponent
    {
        public TransformComponent(Entity entity) : base(entity)
        {
        }
        
        public Matrix WorldMatrix = Matrix.Identity;
        
        public Matrix LocalMatrix = Matrix.Identity;

        public Vector3 Position;

        public Quaternion Rotation;

        public Vector3 Scale;
    }
}