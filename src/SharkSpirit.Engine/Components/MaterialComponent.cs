using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class MaterialComponent : EntityComponent
    {
        public MaterialComponent(Entity entity) : base(entity)
        {
        }

        public Vector4 Color { get; set; }
    }
}
