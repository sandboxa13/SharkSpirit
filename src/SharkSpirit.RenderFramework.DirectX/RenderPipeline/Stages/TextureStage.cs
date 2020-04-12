using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class TextureStage : StageBase
    {
        private readonly ShaderResourceView _shaderResourceView;

        public TextureStage(IDevice device, string path) : base(device)
        {
            var texture = TextureLoader.CreateTexture2DFromBitmap(device.GetDevice(), TextureLoader.LoadBitmap(new SharpDX.WIC.ImagingFactory2(), path));

            _shaderResourceView = new ShaderResourceView(device.GetDevice(), texture);
        }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().PixelShader.SetShaderResource(0, _shaderResourceView);
        }
    }
}
