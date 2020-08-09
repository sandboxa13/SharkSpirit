using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems.Input;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public abstract class ScriptBase : EntityComponent
    {
        public ScriptBase(IContainer container, Entity entity, string path) : base(container, entity, ComponentType.Script, "Script component")
        {
            Input = container.GetService<InputSystem>();
            Entity = entity;
            PathToScript = path;
            
            IsEnabled = true;
        }
        
        public ScriptBase(IContainer container, Entity entity, CameraComponent cameraComponent, string path) : base(container, entity, ComponentType.Script, "Script component")
        {
            Input = container.GetService<InputSystem>();
            PathToScript = path;
            
            //todo get current camera from scene
            Camera = cameraComponent;
            
            Entity = entity;

            IsEnabled = true;
        }
        
        public CameraComponent Camera { get; protected set; }
        public InputSystem Input { get; protected set; }
        public string PathToScript { get; protected set; }
        public bool IsEnabled { get; private set; }

        public abstract void Execute();

        public virtual void ChangeIsEnabled(bool isEnabled) => IsEnabled = isEnabled;
    }
}
