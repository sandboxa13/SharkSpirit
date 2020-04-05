using SharkSpirit.Core;
using SharpDX.DirectInput;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class CameraMoveScript : ScriptsBase
    {
        public override void Execute()
        {
            if (Input.InputManager.ScrollDirection() == ScrollDirection.Forward)
            {
                Camera.Entity.TransformComponent.Position.Z = (Camera.Entity.TransformComponent.Position.Z + 1.5f);
            }
            else if (Input.InputManager.ScrollDirection() == ScrollDirection.Back)
            {
                Camera.Entity.TransformComponent.Position.Z = (Camera.Entity.TransformComponent.Position.Z - 1.5f);
            }
            else if (Input.InputManager.IsPressed(Key.W))
            {
                Camera.Entity.TransformComponent.Position.Y = (Camera.Entity.TransformComponent.Position.Y - 0.5f);
            }
            else if (Input.InputManager.IsPressed(Key.S))
            {
                Camera.Entity.TransformComponent.Position.Y = (Camera.Entity.TransformComponent.Position.Y + 0.5f);
            }
            else if (Input.InputManager.IsPressed(Key.A))
            {
                Camera.Entity.TransformComponent.Position.X = (Camera.Entity.TransformComponent.Position.X - 0.5f);
            }
            else if (Input.InputManager.IsPressed(Key.D))
            {
                Camera.Entity.TransformComponent.Position.X = (Camera.Entity.TransformComponent.Position.X + 0.5f);
            }
            else if (Input.InputManager.IsPressed(Key.Q))
            {
                Camera.Entity.TransformComponent.Rotation.Y = (Camera.Entity.TransformComponent.Rotation.X + 0.5f);
            }
            else if (Input.InputManager.IsPressed(Key.E))
            {
                Camera.Entity.TransformComponent.Rotation.Y = (Camera.Entity.TransformComponent.Rotation.Y - 0.5f);
            }
        }
    }
}
