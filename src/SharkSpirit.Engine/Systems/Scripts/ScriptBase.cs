using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems.Input;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public abstract class ScriptBase : EntityComponent
    {
        public CameraComponent Camera { get; protected set; }
        public InputSystem Input { get; protected set; }

        public abstract void Execute();

        public ScriptBase(IContainer container, Entity entity) : base(entity)
        {
            Input = container.GetService<InputSystem>();
            Entity = entity;
        }
        
        public ScriptBase(IContainer container, Entity entity, CameraComponent cameraComponent) : base(entity)
        {
            Input = container.GetService<InputSystem>();
            Camera = cameraComponent;
            Entity = entity;
        }
    }
}
