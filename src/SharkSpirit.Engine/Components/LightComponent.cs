using SharkSpirit.Core;

namespace SharkSpirit.Engine.Components
{
    public class LightComponent : EntityComponent
    {
        public LightComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Light, "Light component")
        {
        }
    }
}