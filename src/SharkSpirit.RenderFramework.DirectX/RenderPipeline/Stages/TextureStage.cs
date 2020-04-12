using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class TextureStage : StageBase
    {
        private readonly ShaderResourceView _shaderResourceView;

        public TextureStage(IDevice device) : base(device)
        {
            var texture = TextureLoader.CreateTexture2DFromBitmap(device.GetDevice(), TextureLoader.LoadBitmap(new SharpDX.WIC.ImagingFactory2(), "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders\\cube.png"));

            _shaderResourceView = new ShaderResourceView(device.GetDevice(), texture);
        }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().PixelShader.SetShaderResource(0, _shaderResourceView);
        }
    }
}
