using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class SamplerStage : StageBase
    {
        private readonly SamplerState _samplerState;
        public SamplerStage(IDevice device) : base(device)
        {
            var samplerStateDescription = new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                Filter = Filter.MinMagMipLinear
            };

            _samplerState = new SamplerState(device.GetDevice(), samplerStateDescription);
        }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().PixelShader.SetSampler(0, _samplerState);
        }
    }
}
