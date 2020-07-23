using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class MaterialComponent : EntityComponent
    {
        public MaterialComponent(Entity entity) : base(entity)
        {
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public Vector4 Color { get; set; }
    }
}
