using System.Collections.Generic;
using SharkSpirit.Core;
using SharkSpirit.Engine.Components;
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
            foreach (var renderObject in RenderObjects)
            {
                Update(renderObject.Key);
                Update(renderObject.Key, renderObject.Value);
            }
            
            DrawObjects();
        }


        public void RemoveRenderObject(Entity entity)
        {
            RenderObjects.Remove(entity);
        }

        private void Update(Entity entity)
        {
            entity.ApplyTransform(entity.TransformComponent);
        }
        
        private void Update(Entity entity, RenderObject renderObject)
        {
            renderObject.ChangeIsVisible(entity.IsVisible);
            renderObject.UpdateWorld(entity.TransformComponent.WorldMatrix);
            renderObject.UpdateView(_scene.SelectedCamera.ViewMatrix);
            renderObject.UpdateViewProjection(_scene.RenderSystem.Device.GetProjection());
            renderObject.UpdateColor(entity.MaterialComponent.Color);
            renderObject.UpdatePosition(entity.TransformComponent.Position);
            // renderObject.UpdateSpecularIntensity(entity.MaterialComponent.SpecularIntensity);
            // renderObject.UpdateSpecularPower(entity.MaterialComponent.SpecularPower);

            if (!(entity.GetComponent(ComponentType.Light) is LightComponent lightComp)) return;
            
            UpdateLight(renderObject, lightComp);
        }

        private static void UpdateLight(RenderObject renderObject, LightComponent lightComp)
        {
            if (!(renderObject is PointLight pl)) return;

            pl.Ambient = lightComp.Ambient;
            pl.AttConst = lightComp.AttConst;
            pl.AttLin = lightComp.AttLin;
            pl.AttQuad = lightComp.AttQuad;
            pl.DiffuseColor = lightComp.DiffuseColor;
            pl.DiffuseIntensity = lightComp.DiffuseIntensity;
        }

        private void DrawObjects()
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.Value.Draw();
            }
        }
    }
}