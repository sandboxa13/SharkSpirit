using SharkSpirit.RenderFramework.DirectX.RenderPipeline;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class RenderObject
    {
        protected readonly IRenderPipeline _renderPipeline;
        public RenderObject(IDevice device, MeshType meshType)
        {
            Device = device;
            MeshType = meshType;

            _renderPipeline = new RenderPipeline.RenderPipeline(device);
        }

        protected IndexBufferStage IndexBufferStage;
        protected IDevice Device;
        protected internal bool IsVisible;
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

        public Matrix World { get; protected internal set; }
        public Matrix ViewProjection { get; protected internal set; }
        public Matrix View { get; protected internal set; }
        public Vector4 Color { get; protected internal set; }
        public Vector3 Position { get; protected internal set; }

        public virtual void ChangeIsVisible(bool isVisible) => IsVisible = isVisible;
        public virtual void UpdateWorld(Matrix world) => World = world;
        public virtual void UpdateViewProjection(Matrix viewProjection) => ViewProjection = viewProjection;
        public virtual void UpdateView(Matrix view) => View = view;
        public virtual void UpdateColor(Vector4 color) => Color = color;
        public virtual void UpdatePosition(Vector3 position) => Position = position;
        public virtual void Draw()
        {
            if (!IsVisible)
                return;

            _renderPipeline.Bind();

            if (IndexBufferStage == null)
                return;

            Device.GetDeviceContext().DrawIndexed(IndexBufferStage.GetCount(), 0, 0);
        }
    }
}