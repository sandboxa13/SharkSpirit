using System.Collections.Generic;
using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.RenderEngine;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase
    {
        public Scene(IContainer container) : base("Default scene")
        {
            Initialize(container);
        }

        public RenderSystem RenderSystem { get; private set; }

        public List<Entity> Entities { get; private set; }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
        }

        private void Initialize(IContainer container)
        {
            RenderSystem.CreateRenderSystem(container);
        }
    }
}
