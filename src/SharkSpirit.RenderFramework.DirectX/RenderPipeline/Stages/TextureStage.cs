using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class TextureStage : StageBase
    {
        private readonly ShaderResourceView _shaderResourceView;

        public TextureStage(IDevice device, string path, int slot = 0) : base(device)
        {
            Slot = slot;
            var texture = TextureLoader.CreateTexture2DFromBitmap(device.GetDevice(), TextureLoader.LoadBitmap(new SharpDX.WIC.ImagingFactory2(), path));

            _shaderResourceView = new ShaderResourceView(device.GetDevice(), texture);
        }
        
        public int Slot { get; }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().PixelShader.SetShaderResource(Slot, _shaderResourceView);
        }
    }
}
