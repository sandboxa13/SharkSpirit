using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.Primitives;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class PointLight : RenderObject
    {
        private LightCBuf _lightCBuf;
        private readonly PixelConstantBufferStage<LightCBuf> _pixelConstantBufferStage;
        public PointLight(IDevice device, IConfiguration configuration) : base(device, MeshType.None)
        {
            PointLightModel = PrimitivesFactory.CreateSolidSphere(device, configuration);

            _lightCBuf = new LightCBuf
            {
                LightPos = new Vector3(0,0 ,0),
                Ambient = new Vector3(0.05f, 0.05f, 0.05f),
                DiffuseColor = new Vector3(1.0f, 0.0f, 0.0f),
                DiffuseIntensity = 1.0f,
                AttConst = 1.0f,
                AttLin = 0.045f,
                //AttQuad = 0.0075f
            };

            _pixelConstantBufferStage = new PixelConstantBufferStage<LightCBuf>(device, PointLightModel);
        }

        public override void ChangeIsVisible(bool isVisible) => PointLightModel.IsVisible = isVisible;
        public override void UpdateWorld(Matrix world) => PointLightModel.World = world;
        public override void UpdateViewProjection(Matrix viewProjection) => PointLightModel.ViewProjection = viewProjection;
        public override void UpdateView(Matrix view) => PointLightModel.View = view;
        public override void UpdateColor(Vector4 color) => PointLightModel.Color = color;
        public override void UpdatePosition(Vector3 position) => PointLightModel.Position = position;

        public override void Draw()
        {
            var lightPos = Vector3.Transform(PointLightModel.Position, PointLightModel.View);

            _lightCBuf = new LightCBuf
            {
                LightPos = (Vector3)lightPos,
                Ambient = new Vector3(0.05f, 0.05f, 0.05f),
                DiffuseColor = new Vector3(1.0f, 0.0f, 0.0f),
                DiffuseIntensity = 1.0f,
                AttConst = 1.0f,
                AttLin = 1f,
                //AttQuad = 1f
            };

            _pixelConstantBufferStage.BindCustom(_lightCBuf);

            PointLightModel.Draw();
        }

        public RenderObject PointLightModel { get; set; }
    }

    public struct ObjectCBuf
    {
        public Vector3 MaterialColor;
        public float SpecularIntensity;
        public float SpecularPower;
    }
}

