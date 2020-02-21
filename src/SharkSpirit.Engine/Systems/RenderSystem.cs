using System;
using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine.Systems
{
    public class RenderSystem : ComponentBase
    {
        internal RenderSystem(IDevice device)
        {
            Device = device;
            EntityRenderProcessor = new EntityRenderProcessor();
        }

        public IDevice Device { get; }
        public EntityRenderProcessor EntityRenderProcessor { get; private set; }

        public void Draw()
        {
            EntityRenderProcessor.Draw();
        }
    }
}
