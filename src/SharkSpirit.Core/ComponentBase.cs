using System;

namespace SharkSpirit.Core
{
    public class ComponentBase : IDisposable
    {
        public ComponentBase(IContainer container, string name)
        {
            Name = name;
            Container = container;
            IsVisible = true;
        }

        public string Name { get; private set; }
        public IContainer Container { get; }
        public bool IsVisible { get; protected set; }
        public void ChangeIsVisible(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public void Dispose()
        {
            
        }
    }
}
