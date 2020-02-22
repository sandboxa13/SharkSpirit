using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase, IScene
    {
        public Scene(IContainer container) : base("Default scene")
        {
            Initialize(container);
        }

        public CameraComponent CameraComponent { get; set; }
        public RenderSystem RenderSystem { get; private set; }

        public FastCollection<Entity> Entities { get; private set; }

        public void Draw()
        {
            CameraComponent.Update();
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
            CameraComponent = new CameraComponent();
            container.RegisterInstance<IScene>(this);
        }
    }

    public interface IScene
    {
        CameraComponent CameraComponent { get; set; }
    }
}