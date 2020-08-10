using SharkSpirit.RenderFramework.DirectX.RenderPipeline;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class RenderObject
    {
        public RenderObject(IDevice device)
        {
            Device = device;

            RenderPipeline = new RenderPipeline.RenderPipeline(device);
        }

        protected IndexBufferStage IndexBufferStage;
        protected IDevice Device;
        protected internal bool IsVisible;
        protected readonly IRenderPipeline RenderPipeline;

        public void AddStage(StageBase stage)
        {
            if (stage.IsIndexBuffer)
            {
                IndexBufferStage = (IndexBufferStage) stage;
            }
            
            RenderPipeline.AddStage(stage);
        }

        public Matrix World { get; protected internal set; }
        public Matrix ViewProjection { get; protected internal set; }
        public Matrix View { get; protected internal set; }
        public Vector4 Color { get; protected internal set; }
        public Vector3 Position { get; protected internal set; }
        public float SpecularIntensity { get; protected internal set; }
        public float SpecularPower { get; protected internal set; }

        public virtual void ChangeIsVisible(bool isVisible) => IsVisible = isVisible;
        public virtual void UpdateWorld(Matrix world) => World = world;
        public virtual void UpdateViewProjection(Matrix viewProjection) => ViewProjection = viewProjection;
        public virtual void UpdateView(Matrix view) => View = view;
        public virtual void UpdateColor(Vector4 color) => Color = color;
        public virtual void UpdatePosition(Vector3 position) => Position = position;
        public virtual void UpdateSpecularIntensity(float specularIntensity) => SpecularIntensity = specularIntensity;
        public virtual void UpdateSpecularPower(float specularPower) => SpecularPower = specularPower;
        
        public virtual void Draw()
        {
            if (!IsVisible)
                return;

            RenderPipeline.Bind();

            if (IndexBufferStage == null)
                return;

            Device.GetDeviceContext().DrawIndexed(IndexBufferStage.GetCount(), 0, 0);
        }
    }
}