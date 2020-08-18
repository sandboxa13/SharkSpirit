using System;
using SharkSpirit.Core;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class CameraComponent : EntityComponent
    {
        public CameraComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Camera)
        {
        }
        
        public Matrix ViewMatrix;
        
        public bool IsSelected { get; private set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        
        public void Rotate(float dx, float dy)
        {
            // var twoPi = Math.PI * 2;
            // var mod = (Yaw + dx * 12.0f) % twoPi;
            // if (mod > Math.PI)
            // {
                Yaw = Yaw + dx * 12.0f;
            // }
            // else
            // {
            //     Yaw = Yaw + dx * 12.0f;
            // }
            
            Pitch = Clamp(Pitch + dy * 12.0f, (float) (-Math.PI / 2.0f), (float) (Math.PI / 2.0f));
        }

        public void Translate(Vector3 translation)
        {
            var tmp = Vector3.Transform(translation,
                Matrix.RotationYawPitchRoll(Yaw, Pitch, 0.0f) * Matrix.Scaling(0.1f, 0.1f, 0.1f));

            Entity.TransformComponent.Position.X += tmp.X;
            Entity.TransformComponent.Position.Y += tmp.Y;
            Entity.TransformComponent.Position.Z += tmp.Z;
        }
        
        public void Update()
        {
            var lookVector = Vector3.Transform( Vector3.UnitZ, Matrix.RotationYawPitchRoll(Yaw, Pitch, 0.0f));

            var camTarget = Entity.TransformComponent.Position + (Vector3) lookVector;
            
            ViewMatrix = Matrix.LookAtLH(Entity.TransformComponent.Position, camTarget, Vector3.UnitY);
        }

        public void Select() => IsSelected = true;
        public void UnSelect() => IsSelected = false;
        
        private float Clamp(float x, float low, float high)
        {
            return x < low ? low : (x > high ? high : x);
        }
    }
}