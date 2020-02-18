namespace SharkSpirit.Core
{
    public class Configuration : IConfiguration
    {
        public EngineEditorType EngineEditorType { get; set; }
    }

    public interface IConfiguration
    {
        EngineEditorType EngineEditorType { get; set; }
    }
}
