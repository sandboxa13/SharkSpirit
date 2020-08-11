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
        public PointLight(IDevice device, IConfiguration configuration) : base(device)
        {
            PointLightModel = PrimitivesFactory.CreateSolidSphere(device, configuration);

            _lightCBuf = new LightCBuf
            {
                LightPos = new Vector3(0, 0 ,0),
                Ambient = new Vector3(0.15f, 0.15f, 0.15f),
                DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f),
                DiffuseIntensity = 1.0f,
                AttConst = 1.0f,
                AttLin = 0.045f,
                AttQuad = 0.0075f
            };

            _pixelConstantBufferStage = new PixelConstantBufferStage<LightCBuf>(device);
        }

        public override void ChangeIsVisible(bool isVisible) => ChangeIsVisibleInternal(isVisible);
        public override void UpdateWorld(Matrix world) => PointLightModel.World = world;
        public override void UpdateViewProjection(Matrix viewProjection) => PointLightModel.ViewProjection = viewProjection;
        public override void UpdateView(Matrix view) => PointLightModel.View = view;
        public override void UpdateColor(Vector4 color) => PointLightModel.Color = color;
        public override void UpdatePosition(Vector3 position) => PointLightModel.Position = position;
        public float DiffuseIntensity { get; set; }
        public Vector3 Ambient { get; set; }
        public float AttConst { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public float AttLin { get; set; }
        public float AttQuad { get; set; }
        
        public override void Draw()
        {
            if (!IsVisible)
            {
                _pixelConstantBufferStage.BindCustom(_lightCBuf);
                return;
            }
            
            PointLightModel.Draw();

            var lightPos = Vector3.Transform(PointLightModel.Position, PointLightModel.View);

            _lightCBuf.LightPos = (Vector3) lightPos;
            _lightCBuf.Ambient = Ambient;
            _lightCBuf.AttConst = AttConst;
            _lightCBuf.AttLin = AttLin;
            _lightCBuf.AttQuad = AttQuad;
            _lightCBuf.DiffuseColor = DiffuseColor;
            _lightCBuf.DiffuseIntensity = DiffuseIntensity;

            _pixelConstantBufferStage.BindCustom(_lightCBuf);
        }

        public RenderObject PointLightModel { get; set; }
        
        private void ChangeIsVisibleInternal(bool isVisible)
        {
            PointLightModel.IsVisible = isVisible;
            IsVisible = isVisible;

            if (!isVisible)
            {
                _lightCBuf = new LightCBuf
                {
                    LightPos = new Vector3(0, 0, 0),
                    Ambient = new Vector3(0, 0, 0f),
                    DiffuseColor = new Vector3(0.0f, 0.0f, 0.0f),
                    DiffuseIntensity = 0.0f,
                    AttConst = 0.0f,
                    AttLin = 0.0f,
                    AttQuad = 0.0f
                };
            }
            else
            {
                _lightCBuf = new LightCBuf
                {
                    LightPos = new Vector3(0, 0, 0),
                    Ambient = new Vector3(0.05f, 0.05f, 0.05f),
                    DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f),
                    DiffuseIntensity = 1.0f,
                    AttConst = 1.0f,
                    AttLin = 0.045f,
                    AttQuad = 0.0075f
                };
            }
        }
    }
}

