using System;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems;
using SharkSpirit.Engine.Systems.Input;
using SharkSpirit.Engine.Systems.Scripts;
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
        public ScriptSystem ScriptSystem { get; set; }
        public InputSystem InputSystem { get; set; }
        public FpsSystem FpsSystem { get; set; }
        public IConfiguration Configuration { get; private set; }
        public FastCollection<Entity> Entities { get; private set; }

        public void Draw()
        {
            InputSystem.UpdateInput();

            ScriptSystem.ExecuteScripts();

            CameraComponent.Update();

            RenderSystem.Clear();
            RenderSystem.Draw();
            RenderSystem.Flush();
        }

        public void Draw(GameTimer timer)
        {
            InputSystem.UpdateInput();

            ScriptSystem.ExecuteScripts();

            CameraComponent.Update();

           

            RenderSystem.Clear(timer);
            RenderSystem.Draw();

            FpsSystem.Tick();

            var output = string.Join(Environment.NewLine, 
                "SCENE INFO ", 
                "",
                $"FPS : {FpsSystem.GetFps()}", 
                $"FRAME TIME : {FpsSystem.GetMspf()} (ms)", 
                $"MOUSE X : {InputSystem.InputManager.MouseX()}", 
                $"MOUSE Y : {InputSystem.InputManager.MouseY()}",
                $"SCENE OBJECTS COUNT : {Entities.Count} ");

            RenderSystem.DrawSceneInfo(output);
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
            container.AddService(RenderSystem);

            var x = (float) (1.5f * Math.PI);
            var y = (float) (0.2f * Math.PI);
            var z = 15.0f;

            CameraComponent = new CameraComponent(new Entity(new Vector3(x, y, z)));
            var cameraMoveScript = new CameraMoveScript();
            cameraMoveScript.AttachEntity(CameraComponent.Entity);

            container.AddService(CameraComponent);

            Entities = new FastCollection<Entity>();

            ScriptSystem = new ScriptSystem();
            container.AddService(ScriptSystem);

            InputSystem = new InputSystem(container);
            container.AddService(InputSystem);

            cameraMoveScript.Initialize(container);

            ScriptSystem.AddScript(cameraMoveScript);

            FpsSystem = new FpsSystem(1000, container);
            container.AddService(container);
        }
    }

    public interface IScene
    {
        CameraComponent CameraComponent { get; set; }
        RenderSystem RenderSystem { get; set; }
    }
}