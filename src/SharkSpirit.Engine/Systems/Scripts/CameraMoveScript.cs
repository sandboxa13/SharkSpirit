using System;
using SharpDX.DirectInput;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class CameraMoveScript : ScriptsBase
    {
        private float _lastMousePosX;
        private float _lastMousePosY;

        public override void Execute()
        {
            if (Input.InputManager.LMouseDown())
            {
                var dx = ConvertToRadians(0.005f * Input.InputManager.MouseX() - _lastMousePosX);
                var dy = ConvertToRadians(0.005f * Input.InputManager.MouseY() - _lastMousePosY);

                Camera.Entity.TransformComponent.Position.X += dx;
                Camera.Entity.TransformComponent.Position.Y += dy;


                Camera.Entity.TransformComponent.Position.Y =
                    Clamp(Camera.Entity.TransformComponent.Position.Y, 0.1f, (float) (Math.PI - 0.1f));
            }
            else if(Input.InputManager.RMouseDown())
            {
                var dx = ConvertToRadians(0.05f * Input.InputManager.MouseX() - _lastMousePosX);
                var dy = ConvertToRadians(0.05f * Input.InputManager.MouseY() - _lastMousePosY);

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

            _lastMousePosX = Input.InputManager.MouseX();
            _lastMousePosY = Input.InputManager.MouseY();
        }

        private float Clamp(float x, float low, float high)
        {
            return x < low ? low : (x > high ? high : x);
        }

        private float ConvertToRadians(float fDegrees) { return (float) (fDegrees * (Math.PI / 180.0f)); }
    }
}
