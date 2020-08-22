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

        public void Rotate(float dx, float dy)
        {
            Entity.TransformComponent.Rotation.Y = Entity.TransformComponent.Rotation.Y + dx * 12.0f;
            Entity.TransformComponent.Rotation.Z = Clamp(Entity.TransformComponent.Rotation.Z + dy * 12.0f, (float) (-Math.PI / 2.0f), (float) (Math.PI / 2.0f));
        }

        public void Translate(Vector3 translation)
        {
            var tmp = Vector3.Transform(translation,
                Matrix.RotationYawPitchRoll(Entity.TransformComponent.Rotation.Y, Entity.TransformComponent.Rotation.Z, 0.0f) * Matrix.Scaling(0.11f, 0.11f, 0.11f));

            Entity.TransformComponent.Position.X += tmp.X;
            Entity.TransformComponent.Position.Y += tmp.Y;
            Entity.TransformComponent.Position.Z += tmp.Z;
        }

        public void Update()
        {
            var lookVector = Vector3.Transform(Vector3.UnitZ, Matrix.RotationYawPitchRoll(Entity.TransformComponent.Rotation.Y, Entity.TransformComponent.Rotation.Z, 0.0f));

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