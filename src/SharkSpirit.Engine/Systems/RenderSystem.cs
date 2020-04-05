using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine.Systems
{
    public class RenderSystem : ComponentBase
    {
        internal RenderSystem(
            IDevice device, 
            IScene scene)
        {
            Device = device;
            EntityRenderProcessor = new EntityRenderProcessor(scene);
        }

        public IDevice Device { get; }
        public EntityRenderProcessor EntityRenderProcessor { get; private set; }

        public void Draw()
        {
            EntityRenderProcessor.Draw();
        }

        public void Clear()
        {
            Device.Clear();
        }

        public void Flush()
        {
            Device.Flush();
        }

        public void Clear(GameTimer timer)
        {
            Device.Clear(timer.TotalTime);
        }

        public void Reinitialize()
        {
            Device.Reinitialize();
        }
    }
}
