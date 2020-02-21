namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public abstract class BindableBase
    {
        public BindableBase(IDevice device)
        {
            Device = device;
        }
        
        protected IDevice Device { get; private set; }
        
        public abstract void Bind();
    }
}