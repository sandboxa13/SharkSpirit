using System.Drawing;

namespace SharkSpirit.Core
{
    public class Configuration : IConfiguration
    {
        public EngineEditorType EngineEditorType { get; set; }
        public string PathToShaders { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float MonitorWidth { get; set; }
        public float MonitorHeight { get; set; }
        public Rectangle ControlBounds { get; set; }
    }

    public interface IConfiguration
    {
        EngineEditorType EngineEditorType { get; set; }
        string PathToShaders { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        float MonitorWidth { get; set; }
        float MonitorHeight { get; set; }
    }
}
