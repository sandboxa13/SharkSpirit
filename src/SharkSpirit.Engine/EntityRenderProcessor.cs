using System.Collections.Generic;
using SharkSpirit.RenderFramework.DirectX;
using SharpDX;

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
            foreach (var  renderObject in RenderObjects)
            {
                Update( renderObject.Key, renderObject.Value);
            }
            
            DrawObjects();
        }

        public void RemoveRenderObject(Entity entity)
        {
            RenderObjects.Remove(entity);
        }

        private void Update(Entity entity, RenderObject renderObject)
        {
            renderObject.ChangeIsVisible(entity.IsVisible);
            renderObject.UpdateWorld(entity.TransformComponent.WorldMatrix);
            renderObject.UpdateView(_scene.SelectedCamera.ViewMatrix);
            renderObject.UpdateViewProjection(Matrix.Multiply(_scene.SelectedCamera.ViewMatrix, _scene.RenderSystem.Device.GetProjection()));
        }
        
        private void DrawObjects()
        {
            foreach (var  renderObject in RenderObjects)
            {
                renderObject.Value.Draw();
            }
        }
    }
}