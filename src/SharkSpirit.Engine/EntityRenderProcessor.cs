using System.Collections.Generic;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine
{
    public class EntityRenderProcessor
    {
        public readonly Dictionary<Entity, RenderObject> RenderObjects = new Dictionary<Entity, RenderObject>();

        public void AddRenderObject(Entity entity, RenderObject renderObject)
        {
            RenderObjects.Add(entity, renderObject);
        }

        public void Draw()
        {
            foreach (var (entity, renderObject) in RenderObjects)
            {
                Update(entity, renderObject);
            }
        }

        private void Update(Entity entity, RenderObject renderObject)
        {
            renderObject.UpdateTransform(entity.TransformComponent.WorldMatrix);
        }

        public void RemoveRenderObject(Entity entity)
        {
            RenderObjects.Remove(entity);
        }
    }
}