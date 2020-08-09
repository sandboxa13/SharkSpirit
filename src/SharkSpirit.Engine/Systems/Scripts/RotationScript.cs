using SharkSpirit.Core;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class RotationScript : ScriptBase
    {
        private bool _hasEnd;

        public override void Execute()
        {
            if(!IsEnabled)
                return;
            
            if (Entity.TransformComponent.Rotation.Y >= 180)
            {
                _hasEnd = true;
            }
            
            if (Entity.TransformComponent.Rotation.Y < 180 && !_hasEnd)
            {
                Entity.TransformComponent.Rotation.Y += 0.01f ;
            }
            else if ((Entity.TransformComponent.Rotation.Y < -180 || Entity.TransformComponent.Position.Y > -180) && _hasEnd)
            {
                Entity.TransformComponent.Rotation.Y -= 0.01f;
                
                if (Entity.TransformComponent.Rotation.Y <= -180)
                {
                    _hasEnd = false;
                }
            }
        }

        public RotationScript(IContainer container, Entity entity) : base(container, entity, "Rotation script")
        {
        }
    }
}