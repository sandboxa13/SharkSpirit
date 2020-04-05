using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems;
using SharkSpirit.RenderFramework.DirectX;
using SharpDX;
using Configuration = SharkSpirit.Core.Configuration;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase, IScene
    {
        public Scene(IContainer container) : base("Default scene")
        {
            Initialize(container);
        }

        public CameraComponent CameraComponent { get; set; }
        public RenderSystem RenderSystem { get; set; }
        public IConfiguration Configuration { get; private set; }
        public FastCollection<Entity> Entities { get; private set; }

        public void Draw()
        {
            CameraComponent.Update();

            RenderSystem.Clear();
            RenderSystem.Draw();
            RenderSystem.Flush();
        }

        public void Draw(GameTimer timer)
        {
            CameraComponent.Update();

            RenderSystem.Clear(timer);
            RenderSystem.Draw();
            RenderSystem.Flush();
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            RenderSystem.EntityRenderProcessor.AddRenderObject(entity, new Cube(RenderSystem.Device, Configuration));
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            RenderSystem.EntityRenderProcessor.RemoveRenderObject(entity);
        }

        private void Initialize(IContainer container)
        {
            Configuration = container.GetService<Configuration>();
            container.AddService<IScene>(this);

            RenderSystem = RenderSystemFactory.CreateRenderSystem(container, Configuration);
            CameraComponent = new CameraComponent(new Entity(new Vector3(-8, 8, -13)));
            Entities = new FastCollection<Entity>();
        }

        
    }

    public interface IScene
    {
        CameraComponent CameraComponent { get; set; }
        RenderSystem RenderSystem { get; set; }
    }
}