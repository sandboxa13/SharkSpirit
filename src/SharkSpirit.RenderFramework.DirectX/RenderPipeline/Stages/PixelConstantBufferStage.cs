using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly RenderObject _renderObject;
        private float tmp;
        public PixelConstantBufferStage(IDevice device, RenderObject renderObject) : base(device)
        {
            _renderObject = renderObject;
        }

        public override void BindToPipeline()
        {
            //var color = new SpherePrimitiveBuilder.ConstantColor(_renderObject.Color);

            //Update(color);

            //tmp += 0.1f;


            var cb = new LightCBuf
            {
                LightPos = _renderObject.Position,
                //Ambient = new Vector3(0.05f, 0.05f, 0.05f),
                //DiffuseColor = new Vector3(0.0f, 0.0f, 0.0f),
                //DiffuseIntensity = 1.0f,
                //AttConst = 1.0f,
                //AttLin = 1f,
                //AttQuad = 1f
            };

            Update(cb);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}