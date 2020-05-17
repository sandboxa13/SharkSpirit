using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems.Input;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public abstract class ScriptsBase
    {
        public CameraComponent Camera { get; protected set; }
        public Entity Entity { get; protected set; }
        public InputSystem Input { get; protected set; }

        public abstract void Execute();

        public void Initialize(IContainer container)
        {
            Input = container.GetService<InputSystem>();
        }

        public void Initialize(IContainer container, CameraComponent cameraComponent)
        {
            Input = container.GetService<InputSystem>();
            Camera = cameraComponent;
        }

        public void AttachEntity(Entity entity)
        {
            Entity = entity;
        }
    }
}
