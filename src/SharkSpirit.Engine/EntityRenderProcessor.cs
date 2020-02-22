using System.Collections.Generic;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine
{
    public class EntityRenderProcessor
    {
        private readonly IScene _scene;
        public readonly Dictionary<Entity, RenderObject> RenderObjects = new Dictionary<Entity, RenderObject>();

        public EntityRenderProcessor(IScene scene)
        {
            _scene = scene;
        }
        
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
            
            DrawObjects();
        }

        public void RemoveRenderObject(Entity entity)
        {
            RenderObjects.Remove(entity);
        }

        private void Update(Entity entity, RenderObject renderObject)
        {
            renderObject.UpdateWorld(entity.TransformComponent.WorldMatrix);
            renderObject.UpdateView(_scene.CameraComponent.ViewMatrix);
        }
        
        private void DrawObjects()
        {
            foreach (var (_, renderObject) in RenderObjects)
            {
                renderObject.Draw();
            }
        }
    }
}