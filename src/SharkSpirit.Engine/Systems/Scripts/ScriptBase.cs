using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems.Input;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public abstract class ScriptBase : ComponentBase
    {
        public CameraComponent Camera { get; protected set; }
        public Entity Entity { get; protected set; }
        public InputSystem Input { get; protected set; }

        public abstract void Execute();



        public void AttachEntity(Entity entity)
        {
            Entity = entity;
        }

        public ScriptBase(IContainer container, string name) : base(container, name)
        {
            Input = container.GetService<InputSystem>();
        }
        
        public ScriptBase(IContainer container, string name, CameraComponent cameraComponent) : base(container, name)
        {
            Input = container.GetService<InputSystem>();
            Camera = cameraComponent;
        }
    }
}
