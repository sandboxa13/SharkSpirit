using System;
using System.Collections.Generic;
using System.Linq;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharpDX;

namespace SharkSpirit.Engine
{
    public class Entity 
    {

        public Entity(Vector3 position, IContainer container, string name) 
        {
            Components = new FastCollection<EntityComponent>();
            Id = Guid.NewGuid();
            Childs = new List<Entity>();
            Name = name;
            Container = container;
            IsVisible = true;
            
            TransformComponent = new TransformComponent(container, this) { Position = position };
            MaterialComponent = new MaterialComponent(container, this);
            
            AddComponent(TransformComponent);
            AddComponent(MaterialComponent);
        }

        public Entity(IContainer container) 
        {
            Components = new FastCollection<EntityComponent>();
            Id = Guid.NewGuid();
            Childs = new List<Entity>();
            Container = container;
            IsVisible = true;
            
            TransformComponent = new TransformComponent(container, this) { Position = Vector3.Zero };
            MaterialComponent = new MaterialComponent(container,this);
            
            AddComponent(TransformComponent);
            AddComponent(MaterialComponent);
        }
        
        public Entity(IContainer container, string name) 
        {
            Container = container;
            Components = new FastCollection<EntityComponent>();
            Id = Guid.NewGuid();
            Childs = new List<Entity>();
            Name = name;
            IsVisible = true;
            
            TransformComponent = new TransformComponent(container, this) { Position = Vector3.Zero };
            MaterialComponent = new MaterialComponent(container,this);
            
            AddComponent(TransformComponent);
            AddComponent(MaterialComponent);
        }

        public FastCollection<EntityComponent> Components { get; }
        public Guid Id { get; private set; }
        public TransformComponent TransformComponent;
        public MaterialComponent MaterialComponent;
        public List<Entity> Childs { get; set; }
        public IContainer Container { get; }
        
        //todo move to component
        public bool IsVisible { get; set; }
        
        public string Name { get; set; }

        public void AddComponent(EntityComponent entityComponent)
        {
            Components.Add(entityComponent);
        }

        public void RemoveComponent(EntityComponent entityComponent)
        {
            Components.Remove(entityComponent);
        }

        public ComponentBase GetComponent(ComponentType type)
        {
            return Components.FirstOrDefault(component => component.ComponentType == type);
        }

        public static Entity Empty(IContainer container)
        {
            return new Entity(container);
        }

        public void ChangeIsVisible(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public void ApplyTransform(TransformComponent accumulatedTransformComponent)
        {
            TransformComponent.Position = accumulatedTransformComponent.Position;
            TransformComponent.Rotation = accumulatedTransformComponent.Rotation;
            
            foreach (var entity in Childs)
            {
                entity.ApplyTransform(TransformComponent);
            }
        }
    }
}