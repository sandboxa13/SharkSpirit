using System;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharpDX;

namespace SharkSpirit.Engine
{
    public class Entity : ComponentBase
    {
        internal TransformComponent TransformComponent;

        public Entity(Vector3 position)
        {
            Components = new FastCollection<EntityComponent>();

            Id = Guid.NewGuid();
            TransformComponent = new TransformComponent { Position = position };
            Components.Add(TransformComponent);
        }
    
        public FastCollection<EntityComponent> Components { get; }
        public Guid Id { get; set; }

        public void AddComponent(EntityComponent entityComponent)
        {
            Components.Add(entityComponent);
        }

        public void RemoveComponent(EntityComponent entityComponent)
        {
            Components.Remove(entityComponent);
        }
    }
}