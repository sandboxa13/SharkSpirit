using System;

namespace SharkSpirit.Core
{
    public class ComponentBase : IDisposable
    {

        public ComponentBase(IContainer container, string name)
        {
            Name = name;
            Container = container;
        }

        public string Name { get; private set; }
        public IContainer Container { get; }

        public void Dispose()
        {
            
        }
    }
}
