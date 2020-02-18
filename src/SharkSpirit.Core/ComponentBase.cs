using System;

namespace SharkSpirit.Core
{
    public class ComponentBase : IDisposable
    {
        public ComponentBase()
        {
            
        }

        public ComponentBase(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public void Dispose()
        {
            
        }
    }
}
