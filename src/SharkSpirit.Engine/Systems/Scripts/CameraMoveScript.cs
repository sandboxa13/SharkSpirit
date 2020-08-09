using System;
using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
using SharpDX.DirectInput;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class CameraMoveScript : ScriptBase
    {
        private float _lastMousePosX;
        private float _lastMousePosY;

        public override void Execute()
        {
            if(!IsEnabled)
                return;
            
            if(!Camera.IsSelected)
                return;

            var configuration = Entity.Container.GetService<Configuration>();
            var mouseX = Input.InputManager.MouseX();
            var mouseY = Input.InputManager.MouseY();


            if (mouseX < configuration.ControlBounds.X || 
                mouseX > configuration.ControlBounds.Width || 
                mouseY < configuration.ControlBounds.Y ||
                mouseY > configuration.ControlBounds.Height)
                return;


            if (Input.InputManager.LMouseDown())
            {
                var dx = ConvertToRadians(0.005f * Input.InputManager.RawMouseX() - _lastMousePosX);
                var dy = ConvertToRadians(0.005f * Input.InputManager.RawMouseY() - _lastMousePosY);

                Camera.Entity.TransformComponent.Position.X += dx;
                Camera.Entity.TransformComponent.Position.Y += dy;


                Camera.Entity.TransformComponent.Position.Y =
                    Clamp(Camera.Entity.TransformComponent.Position.Y, 0.1f, (float)(Math.PI - 0.1f));
            }
            else if (Input.InputManager.RMouseDown())
            {
                var dx = ConvertToRadians(0.05f * Input.InputManager.RawMouseX() - _lastMousePosX);
                var dy = ConvertToRadians(0.05f * Input.InputManager.RawMouseY() - _lastMousePosY);

                Camera.Entity.TransformComponent.Position.Z += dx - dy;
                Camera.Entity.TransformComponent.Position.Z = Clamp(Camera.Entity.TransformComponent.Position.Z, 5.0f, 150.0f);
            }

            if (Input.InputManager.IsPressed(Key.W))
            {
                Camera.Entity.TransformComponent.Position.Z -= 0.25f;
            }
            if (Input.InputManager.IsPressed(Key.S))
            {
                Camera.Entity.TransformComponent.Position.Z += 0.25f;
            }

            _lastMousePosX = Input.InputManager.RawMouseX();
            _lastMousePosY = Input.InputManager.RawMouseY();
        }

        private float Clamp(float x, float low, float high)
        {
            return x < low ? low : (x > high ? high : x);
        }

        private float ConvertToRadians(float fDegrees) { return (float)(fDegrees * (Math.PI / 180.0f)); }

        public CameraMoveScript(IContainer container, Entity entity, CameraComponent cameraComponent) : base(container, entity,
            cameraComponent)
        {
            
        }
    }
}
