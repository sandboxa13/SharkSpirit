using System;
using System.Text;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems;
using SharkSpirit.Engine.Systems.Input;
using SharkSpirit.Engine.Systems.Scripts;
using SharkSpirit.RenderFramework.DirectX;
using SharkSpirit.RenderFramework.DirectX.Primitives;
using SharpDX;
using Configuration = SharkSpirit.Core.Configuration;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase, IScene
    {
        private readonly IContainer _container;
        private readonly StringBuilder _stringBuilder;
        public Scene(IContainer container) : base(container, "Default scene")
        {
            _container = container;
            _stringBuilder = new StringBuilder();

            Initialize();
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
            // update user input
            InputSystem.UpdateInput();

            // execute scripts
            ScriptSystem.ExecuteScripts();

            // update camera
            CameraComponent.Update();

            // clear context
            RenderSystem.Clear(timer);

            // draw
            RenderSystem.Draw();

            // tick fps 
            FpsSystem.Tick();

            BuildAndDrawSceneInfo();

            
            RenderSystem.Flush();
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            RenderSystem.EntityRenderProcessor.AddRenderObject(entity, PrimitivesFactory.CreateTexturedCube(RenderSystem.Device, Configuration));
        }

        public void AddEntity(Entity entity, PrimitiveDrawableTypes primitiveDrawableType)
        {
            Entities.Add(entity);
            RenderSystem.EntityRenderProcessor.AddRenderObject(entity, PrimitivesFactory.Create(RenderSystem.Device, Configuration, primitiveDrawableType));
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            RenderSystem.EntityRenderProcessor.RemoveRenderObject(entity);
        }
       
        private void Initialize()
        {
            Configuration = Container.GetService<Configuration>();
            Container.AddService<IScene>(this);

            RenderSystem = RenderSystemFactory.CreateRenderSystem(Container, Configuration);
            Container.AddService(RenderSystem);

            var x = (float) (1.5f * Math.PI);
            var y = (float) (0.2f * Math.PI);
            var z = 15.0f;

            CameraComponent = new CameraComponent(new Entity(new Vector3(x, y, z), Container, "Camera"));
            var cameraMoveScript = new CameraMoveScript();
            cameraMoveScript.AttachEntity(CameraComponent.Entity);


            Container.AddService(CameraComponent);

            Entities = new FastCollection<Entity>();

            ScriptSystem = new ScriptSystem();
            Container.AddService(ScriptSystem);

            InputSystem = new InputSystem(Container);
            Container.AddService(InputSystem);

            cameraMoveScript.Initialize(Container);

            ScriptSystem.AddScript(cameraMoveScript);

            FpsSystem = new FpsSystem(60, Container);
            Container.AddService(FpsSystem);
        }

        private void BuildAndDrawSceneInfo()
        {
            _stringBuilder.Append(Environment.NewLine);
            _stringBuilder.Append("RENDER ENGINE INFO \n");
            _stringBuilder.Append($"ACTUAL SCENE SIZE: {Configuration.Width} X {Configuration.Height}\n");
            _stringBuilder.Append($"ACTUAL MONITOR SIZE: {Configuration.MonitorWidth} X {Configuration.MonitorHeight}\n");
            _stringBuilder.Append($"FPS : {FpsSystem.GetFps()}\n");
            _stringBuilder.Append($"FRAME TIME : {FpsSystem.GetMspf()} (ms)\n");
            _stringBuilder.Append($"MOUSE X : {InputSystem.InputManager.MouseX()}\n");
            _stringBuilder.Append($"MOUSE Y : {InputSystem.InputManager.MouseY()}\n");
            _stringBuilder.Append($"SCENE OBJECTS COUNT : {Entities.Count} ");

            RenderSystem.DrawSceneInfo(_stringBuilder.ToString());

            _stringBuilder.Clear();
        }

        public void Reinitialize()
        {
            Configuration = _container.GetService<Configuration>();
            RenderSystem.Reinitialize();
        }
    }

    public interface IScene
    {
        CameraComponent CameraComponent { get; set; }
        RenderSystem RenderSystem { get; set; }
        FastCollection<Entity> Entities { get; }
        void RemoveEntity(Entity entity);
        void AddEntity(Entity entity);
    }
}