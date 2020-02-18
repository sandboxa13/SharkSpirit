using SharkSpirit.Core;

namespace SharkSpirit.Engine
{
    public class Entity : ComponentBase
    {
        public FastCollection<EntityComponent> Components { get; }
    }
}