using SharkSpirit.RenderFramework.DirectX.RenderPipeline;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class RenderObject
    {
        private readonly IRenderPipeline _renderPipeline;
        public RenderObject(IDevice device, MeshType meshType)
        {
            Device = device;
            MeshType = meshType;

            _renderPipeline = new RenderPipeline.RenderPipeline(device);
        }
        
        protected IndexBufferStage IndexBufferStage;
        protected IDevice Device;
        protected bool IsVisible;
        protected MeshType MeshType;

        public void AddStage(StageBase stage)
        {
            _renderPipeline.AddStage(stage);
        }

        public void AddIndexBufferStage(IndexBufferStage indexBufferStage)
        {
            IndexBufferStage = indexBufferStage;

            _renderPipeline.AddStage(indexBufferStage);
        }
        
        public Matrix World { get; private set; }
        public Matrix ViewProjection { get; private set; }
        public Matrix View { get; private set; }
        public Vector4 Color { get; private set; }

        public void ChangeIsVisible(bool isVisible) => IsVisible = isVisible;
        public void UpdateWorld(Matrix world) => World = world;
        public void UpdateViewProjection(Matrix viewProjection) => ViewProjection = viewProjection;
        public void UpdateView(Matrix view) => View = view;
        public void UpdateColor(Vector4 color) => Color = color;
        public void Draw()
        {
            if(!IsVisible)
                return;

            _renderPipeline.Bind();

            Device.GetDeviceContext().DrawIndexed(IndexBufferStage.GetCount(), 0, 0);
        }
    }
}