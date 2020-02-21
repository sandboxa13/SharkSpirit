using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Systems;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase
    {
        public Scene(IContainer container) : base("Default scene")
        {
            Initialize(container);
        }

        public RenderSystem RenderSystem { get; private set; }

        public FastCollection<Entity> Entities { get; private set; }

        public void Draw()
        {
            RenderSystem.Draw();
        }
        
        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            RenderSystem.EntityRenderProcessor.AddRenderObject(entity, new RenderObject());
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            RenderSystem.EntityRenderProcessor.RemoveRenderObject(entity);
        }

        private void Initialize(IContainer container)
        {
            RenderSystemFactory.CreateRenderSystem(container);
        }
    }
}
