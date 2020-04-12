using SharkSpirit.RenderFramework.DirectX.RenderPipeline;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class RenderObject
    {
        private readonly IRenderPipeline _renderPipeline;
        public RenderObject(IDevice device)
        {
            Device = device;

            _renderPipeline = new RenderPipeline.RenderPipeline(device);
        }
        
        protected IndexBufferStage IndexBufferStage;
        protected IDevice Device;
        protected bool IsVisible;

        protected void AddStage(StageBase stage)
        {
            _renderPipeline.AddStage(stage);
        }

        protected void AddIndexBufferStage(IndexBufferStage indexBufferStage)
        {
            IndexBufferStage = indexBufferStage;

            _renderPipeline.AddStage(indexBufferStage);
        }
        
        public Matrix World { get; private set; }
        public Matrix ViewProjection { get; private set; }
        public Matrix View { get; private set; }

        public void ChangeIsVisible(bool isVisible) => IsVisible = isVisible;
        public void UpdateWorld(Matrix world) => World = world;
        public void UpdateViewProjection(Matrix viewProjection) => ViewProjection = viewProjection;
        public void UpdateView(Matrix view) => View = view;

        public void Draw()
        {
            if(!IsVisible)
                return;

            _renderPipeline.Bind();

            Device.GetDeviceContext().DrawIndexed(IndexBufferStage.GetCount(), 0, 0);
        }
    }
}