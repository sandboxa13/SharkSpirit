using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharkSpirit.Core;
using SharkSpirit.Core.Collections;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems;
using SharkSpirit.Engine.Systems.Input;
using SharkSpirit.Engine.Systems.Scripts;
using SharkSpirit.RenderFramework.DirectX;
using SharkSpirit.RenderFramework.DirectX.SceneGraph;
using SharpDX;
using Configuration = SharkSpirit.Core.Configuration;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase, IScene
    {
        private readonly IContainer _container;
        private readonly StringBuilder _stringBuilder;

        public Scene(IContainer container) : base(container, "Default scene", ComponentType.None)
        {
            _container = container;
            _stringBuilder = new StringBuilder();

            Initialize();
        }

        public FastCollection<CameraComponent> Cameras { get; set; }
        public CameraComponent SelectedCamera { get; set; }
        public RenderSystem RenderSystem { get; set; }
        public ScriptSystem ScriptSystem { get; set; }
        public InputSystem InputSystem { get; set; }
        public FpsSystem FpsSystem { get; set; }
        public DiagnosticsSystem DiagnosticsSystem { get; set; }
        public IConfiguration Configuration { get; private set; }
        public FastCollection<Entity> Entities { get; private set; }
        public int VertexCount { get; set; }

        public void Draw(GameTimer timer)
        {
            // update user input
            InputSystem.UpdateInput();

            // execute scripts
            ScriptSystem.ExecuteScripts();

            // update camera
            SelectedCamera.Update();

            // clear context
            RenderSystem.Clear(timer);

            //_model.Draw();

            // draw
            RenderSystem.Draw();

            // tick fps 
            FpsSystem.Tick();

            BuildAndDrawSceneInfo(timer);

            RenderSystem.Flush();
        }

        public void SelectCamera(Entity entity)
        {
            var newSelectedCamera = Cameras.FirstOrDefault(component => component.Entity.Id == entity.Id);

            if (newSelectedCamera == null)
                return;

            SelectedCamera.UnSelect();

            newSelectedCamera.Select();

            SelectedCamera = newSelectedCamera;
        }

        public void AddCamera()
        {
            var x = 3.2f;
            var y = 0.68f;
            var z = 30.3f;

            var camera =
                new CameraComponent(Container, new Entity(new Vector3(x, y, z), Container, $"Camera {Cameras.Count + 1}"));
            var cameraMoveScript = new CameraMoveScript(Container, camera.Entity, camera);
            camera.Entity.AddComponent(cameraMoveScript);


            ScriptSystem.AddScript(cameraMoveScript);

            Cameras.Add(camera);

            SelectCamera(camera.Entity);
        }


        public Task AddEntityAsync(Entity entity)
        {
            entity.TransformComponent.Position.Y = 10;
            entity.TransformComponent.Position.X = -5;
            entity.TransformComponent.Position.Z = 3;
            
            entity.AddComponent(new LightComponent(Container, entity));
            
            Entities.Add(entity);
            RenderSystem.EntityRenderProcessor.AddRenderObject(entity,
                new PointLight(RenderSystem.Device, Configuration));
            
            var rotationScript = new LightMoveScript(Container, entity);
            entity.AddComponent(rotationScript);
                
            ScriptSystem.AddScript(rotationScript);
            
            return Task.CompletedTask;
        }

        public Task LoadModelAsync(string name, float scale, bool useRotationScript = false)
        {
            var model = new Model(RenderSystem.Device, Configuration,
                Path.Combine(Configuration.PathToModels, name), scale);

            var parent = new Entity(Container, model.RootNode.Name);
            
            if (useRotationScript)
            {
                var rotationScript = new RotationScript(Container, parent);
                parent.AddComponent(rotationScript);
                ScriptSystem.AddScript(rotationScript);
            }
            
            RenderSystem.EntityRenderProcessor.AddRenderObject(parent, model);
            Entities.Add(BuildTree(model.RootNode, parent));

            return Task.CompletedTask;
        }

        private Entity BuildTree(Node node, Entity parent)
        {
            foreach (var nodeChild in node.Childs)
            {
                var newParent = new Entity(Container, nodeChild.Name);

                BuildTree(nodeChild, newParent);

                foreach (var nodeMesh in nodeChild.Meshes)
                {
                    VertexCount += nodeMesh.VertexCount;
                    var meshEntity = new Entity(Container, nodeMesh.Name);
                    
                    parent.Childs.Add(meshEntity);

                    RenderSystem.EntityRenderProcessor.AddRenderObject(meshEntity, nodeMesh);
                }
            }

            return parent;
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

            Cameras = new FastCollection<CameraComponent>();

            var x = (float) (1.5f * Math.PI);
            var y = (float) (0.2f * Math.PI);
            var z = 15.0f;

            SelectedCamera = new CameraComponent(Container, new Entity(new Vector3(x, y, z), Container, "Camera 1"));
            SelectedCamera.Select();

            var cameraMoveScript = new CameraMoveScript(Container, SelectedCamera.Entity, SelectedCamera);
            SelectedCamera.Entity.AddComponent(cameraMoveScript);
            Cameras.Add(SelectedCamera);

            Container.AddService(Cameras);

            Entities = new FastCollection<Entity>();

            ScriptSystem = new ScriptSystem();
            Container.AddService(ScriptSystem);

            InputSystem = new InputSystem(Container);
            Container.AddService(InputSystem);


            ScriptSystem.AddScript(cameraMoveScript);

            FpsSystem = new FpsSystem(60, Container);
            Container.AddService(FpsSystem);

            DiagnosticsSystem = new DiagnosticsSystem();
            Container.AddService(DiagnosticsSystem);
        }

        private void BuildAndDrawSceneInfo(GameTimer timer)
        {
            var inf = DiagnosticsSystem.CollectInformation(timer);
            _stringBuilder.Append(Environment.NewLine);
            _stringBuilder.Append("RENDER ENGINE INFO \n");
            _stringBuilder.Append($"ACTUAL SCENE SIZE: {Configuration.Width} X {Configuration.Height}\n");
            _stringBuilder.Append(
                $"ACTUAL MONITOR SIZE: {Configuration.MonitorWidth} X {Configuration.MonitorHeight}\n");
            _stringBuilder.Append($"FPS : {FpsSystem.GetFps()}\n");
            _stringBuilder.Append($"FRAME TIME : {FpsSystem.GetMspf()} (ms)\n");
            _stringBuilder.Append($"MOUSE X : {InputSystem.InputManager.MouseX()}\n");
            _stringBuilder.Append($"MOUSE Y : {InputSystem.InputManager.MouseY()}\n");
            _stringBuilder.Append($"SCENE OBJECTS COUNT : {Entities.Count} \n");
            _stringBuilder.Append($"SCENE VERTEX COUNT : {VertexCount} \n");
            _stringBuilder.Append($"CPU USAGE = {inf.CpuUsage} %\n");
            _stringBuilder.Append($"MEMORY USAGE = {inf.MemoryUsage} MB \n");

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
        FastCollection<CameraComponent> Cameras { get; set; }
        CameraComponent SelectedCamera { get; set; }
        RenderSystem RenderSystem { get; set; }
        FastCollection<Entity> Entities { get; }
        void RemoveEntity(Entity entity);
        Task AddEntityAsync(Entity entity);
        Task LoadModelAsync(string name, float scale, bool useRotationScript);
        void SelectCamera(Entity entity);
        void AddCamera();
    }
}