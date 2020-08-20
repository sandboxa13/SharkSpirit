using System;
using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
using SharpDX;
using SharpDX.DirectInput;
using Configuration = SharkSpirit.Core.Configuration;

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


            if (Input.InputManager.RMouseDown())
            {
                var dx = ConvertToRadians(Input.InputManager.RawMouseX() * 0.9f);
                var dy = ConvertToRadians(Input.InputManager.RawMouseY() * 0.9f);

                Camera.Rotate(dx / 100 , dy / 100);
            }

            var multiplier = 1.0f;

            if (Input.InputManager.IsPressed(Key.LeftShift))
            {
                multiplier *= 10;
            }
            
            if (Input.InputManager.IsPressed(Key.W))
            {
                Camera.Translate(new Vector3(0.0f, 0.0f, 1.0f * multiplier));
            }
            if (Input.InputManager.IsPressed(Key.S))
            {
                Camera.Translate(new Vector3(0.0f, 0.0f, -1.0f * multiplier));
            }
            if (Input.InputManager.IsPressed(Key.D))
            {
                Camera.Translate(new Vector3(1.0f * multiplier, 0.0f, 0.0f ));
            }
            if (Input.InputManager.IsPressed(Key.A))
            {
                Camera.Translate(new Vector3(-1.0f * multiplier, 0.0f, 0.0f));
            }

            _lastMousePosX = Input.InputManager.RawMouseX();
            _lastMousePosY = Input.InputManager.RawMouseY();
        }

        private float Clamp(float x, float low, float high)
        {
            return x < low ? low : (x > high ? high : x);
        }

        private float ConvertToRadians(float fDegrees) { return (float)(fDegrees * (Math.PI / 180.0f)); }

        public CameraMoveScript(IContainer container, Entity entity, CameraComponent cameraComponent) : base(container, entity, cameraComponent, "Camera Move script")
        {
            
        }
    }
}
