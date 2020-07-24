using System;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharpDX;

namespace SharkSpirit.Engine
{
    public class Entity : ComponentBase
    {
        public TransformComponent TransformComponent;
        public MaterialComponent MaterialComponent;

        public Entity(Vector3 position, IContainer container) : base(container, "")
        {
            Components = new FastCollection<EntityComponent>();

            Id = Guid.NewGuid();
            TransformComponent = new TransformComponent (this){ Position = position };
            MaterialComponent = new MaterialComponent(this);
            Components.Add(TransformComponent);
            Components.Add(MaterialComponent);
        }

        public Entity(Vector3 position, IContainer container, string name) : base(container, name)
        {
            Components = new FastCollection<EntityComponent>();

            Id = Guid.NewGuid();
            TransformComponent = new TransformComponent(this) { Position = position };
            MaterialComponent = new MaterialComponent(this);
            Components.Add(TransformComponent);
            Components.Add(MaterialComponent);
        }

        public Entity() : base(new Container(), "")
        {
            Components = new FastCollection<EntityComponent>();
            Id = Guid.NewGuid();
            TransformComponent = new TransformComponent(this) { Position = Vector3.Zero };
            MaterialComponent = new MaterialComponent(this);
            Components.Add(TransformComponent);
            Components.Add(MaterialComponent);
        }
        
        public Entity(string name) : base(new Container(), name)
        {
            Components = new FastCollection<EntityComponent>();
            Id = Guid.NewGuid();
            TransformComponent = new TransformComponent(this) { Position = Vector3.Zero };
            MaterialComponent = new MaterialComponent(this);
            Components.Add(TransformComponent);
            Components.Add(MaterialComponent);
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

        public static Entity Empty()
        {
            return new Entity();
        }
    }
}