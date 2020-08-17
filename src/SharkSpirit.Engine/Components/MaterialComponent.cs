using SharkSpirit.Core;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class MaterialComponent : EntityComponent
    {
        public MaterialComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Material, "Material component")
        {
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            SpecularIntensity = 1.6f;
            SpecularPower = 50.0f;
        }

        public Vector4 Color { get; set; }
        public float SpecularIntensity { get; set; }
        public float SpecularPower { get; set; }
    }
}
