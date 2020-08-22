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
        public CameraMoveScript(IContainer container, Entity entity, CameraComponent cameraComponent) : base(container, entity, cameraComponent, "Camera Move script")
        {
            
        }
        
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
                var dx = ConvertToRadians(Input.InputManager.RawMouseX() * 0.9f) / 100;
                var dy = ConvertToRadians(Input.InputManager.RawMouseY() * 0.9f) / 100;

                Entity.TransformComponent.Rotation.Y = Entity.TransformComponent.Rotation.Y + dx * 12.0f;
                Entity.TransformComponent.Rotation.Z = Clamp(Entity.TransformComponent.Rotation.Z + dy * 12.0f, (float) (-Math.PI / 2.0f), (float) (Math.PI / 2.0f));
            }

            var multiplier = 1.0f;

            if (Input.InputManager.IsPressed(Key.LeftShift))
            {
                multiplier *= 10;
            }
            
            if (Input.InputManager.IsPressed(Key.W))
            {
                Translate(new Vector3(0.0f, 0.0f, 1.0f * multiplier));
            }
            if (Input.InputManager.IsPressed(Key.S))
            {
                Translate(new Vector3(0.0f, 0.0f, -1.0f * multiplier));
            }
            if (Input.InputManager.IsPressed(Key.D))
            {
                Translate(new Vector3(1.0f * multiplier, 0.0f, 0.0f ));
            }
            if (Input.InputManager.IsPressed(Key.A))
            {
                Translate(new Vector3(-1.0f * multiplier, 0.0f, 0.0f));
            }
        }

        private float ConvertToRadians(float fDegrees) { return (float)(fDegrees * (Math.PI / 180.0f)); }
        
        private void Translate(Vector3 translation)
        {
            var tmp = Vector3.Transform(translation,
                Matrix.RotationYawPitchRoll(Entity.TransformComponent.Rotation.Y, Entity.TransformComponent.Rotation.Z, 0.0f) * Matrix.Scaling(0.11f, 0.11f, 0.11f));

            Entity.TransformComponent.Position.X += tmp.X;
            Entity.TransformComponent.Position.Y += tmp.Y;
            Entity.TransformComponent.Position.Z += tmp.Z;
        }
        
        private float Clamp(float x, float low, float high)
        {
            return x < low ? low : (x > high ? high : x);
        }
    }
}
