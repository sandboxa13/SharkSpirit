namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public abstract class StageBase
    {
        protected StageBase(IDevice device)
        {
            Device = device;
        }

        protected IDevice Device { get; private set; }
        
        public bool IsIndexBuffer { get; protected set; }

        public abstract void BindToPipeline();
    }
}
