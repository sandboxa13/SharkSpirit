using SharkSpirit.Core;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class MaterialComponent : EntityComponent
    {
        public MaterialComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Material)
        {
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public Vector4 Color { get; set; }
    }
}
