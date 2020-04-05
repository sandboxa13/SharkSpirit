using System;

namespace SharkSpirit.Engine.Components
{
    public abstract class EntityComponent
    {
        public EntityComponent(Entity entity)
        {
            Entity = entity;
        }
        
        public Entity Entity { get; internal set; }

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}