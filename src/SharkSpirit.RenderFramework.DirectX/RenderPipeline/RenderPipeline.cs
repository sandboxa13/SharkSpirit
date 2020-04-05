using System.Collections.Generic;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline
{
    public class RenderPipeline : IRenderPipeline
    {
        private readonly List<StageBase> _stages;

        public RenderPipeline(IDevice device)
        {
            _stages = new List<StageBase>();

            Device = device;
        }

        protected IDevice Device { get; private set; }

        public void AddStage(StageBase stage)
        {
            _stages.Add(stage);
        }

        public void Bind()
        {
            foreach (var stage in _stages)
            {
                stage.BindToPipeline();
            }
        }
    }

    public interface IRenderPipeline
    {
        void AddStage(StageBase stage);
        void Bind();
    }
}
