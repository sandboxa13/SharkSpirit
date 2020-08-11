using SharkSpirit.Core;
using SharpDX;

namespace SharkSpirit.Engine.Components
{
    public class LightComponent : EntityComponent
    {
        public LightComponent(IContainer container, Entity entity) : base(container, entity, ComponentType.Light, "Light component")
        {
            Ambient = new Vector3(0.05f, 0.05f, 0.05f);
            DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            DiffuseIntensity = 1.0f;
            AttConst = 1.0f;
            AttLin = 0.045f;
            AttQuad = 0.0075f;
        }
        
        public float DiffuseIntensity { get; set; }
        
        public Vector3 Ambient { get; set; }
        
        public float AttConst { get; set; }

        public Vector3 DiffuseColor { get; set; }
        
        public float AttLin { get; set; }
        
        public float AttQuad { get; set; }
    }
}