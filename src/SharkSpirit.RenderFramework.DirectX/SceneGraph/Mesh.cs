using System.Collections.Generic;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.SceneGraph
{
    public class Mesh : RenderObject
    {
        public Mesh(IDevice device, IEnumerable<StageBase> stages, string name, int vertexCount) : base(device)
        {
            Initialize(stages);

            Name = name;
            VertexCount = vertexCount;
            IsDrawCallOverriden = true;

            AddStage(new TransformConstantBufferStage<TransformBuffer>(device, this));
        }
        
        public string Name { get; set; }
        
        public int VertexCount { get; set; }

        private void Initialize(IEnumerable<StageBase> stages)
        {
            foreach (var stageBase in stages)
            {
                AddStage(stageBase);
            }
        }
    }
}