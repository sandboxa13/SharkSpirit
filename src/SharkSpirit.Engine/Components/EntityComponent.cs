using System;
using SharkSpirit.Core;

namespace SharkSpirit.Engine.Components
{
    public abstract class EntityComponent : ComponentBase
    {
        public EntityComponent(IContainer container, Entity entity, ComponentType componentType, string componentName = "")
            : base(container, componentName, componentType)
        {
            Entity = entity;
        }
        
        public Entity Entity { get; internal set; }

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}