using System;
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

            var rotationY = Entity.TransformComponent.Rotation.Y / (Math.PI/180);
            
            if (rotationY >= 180)
            {
                _hasEnd = true;
            }
            
            if (rotationY < 180 && !_hasEnd)
            {
                Entity.TransformComponent.Rotation.Y  += 0.01f ;
            }
            else if ((rotationY < -180 || Entity.TransformComponent.Position.Y / (Math.PI/180) > -180) && _hasEnd)
            {
                Entity.TransformComponent.Rotation.Y -= 0.01f;
                
                if (rotationY <= -180)
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