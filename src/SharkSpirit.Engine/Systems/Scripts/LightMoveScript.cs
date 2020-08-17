using SharkSpirit.Core;

namespace SharkSpirit.Engine.Systems.Scripts
{
    public class LightMoveScript : ScriptBase
    {
        private bool _hasEnd;

        public override void Execute()
        {
            if(!IsEnabled)
                return;
            
            if (Entity.TransformComponent.Position.Y >= 20)
            {
                _hasEnd = true;
            }
            
            
            if (Entity.TransformComponent.Position.Y < 20 && !_hasEnd)
            {
                Entity.TransformComponent.Position.Y += 0.1f ;
            }
            else if ((Entity.TransformComponent.Position.Y < 3 || Entity.TransformComponent.Position.Y > 3) && _hasEnd)
            {
                Entity.TransformComponent.Position.Y -= 0.1f;
                
                if (Entity.TransformComponent.Position.Y <= 3)
                {
                    _hasEnd = false;
                }
            }
        }

        public LightMoveScript(IContainer container, Entity entity) : base(container, entity, "Light Move script")
        {
        }
    }
}