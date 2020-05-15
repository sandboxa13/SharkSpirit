using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class PointLight
    {
        public PointLight(IDevice device)
        {
            PointLightModel = new Sphere(device);
        }

        public RenderObject PointLightModel { get; set; }
    }
}

