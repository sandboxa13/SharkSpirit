using System.Collections.Generic;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;

namespace SharkSpirit.RenderFramework.DirectX.SceneGraph
{
    public class Mesh : RenderObject
    {
        public Mesh(IDevice device, IEnumerable<StageBase> stages, string name, int vertexCount) : base(device)
        {
            Initialize(stages);

            Name = name;
            VertexCount = vertexCount;
            
            AddStage(new TransformConstantBufferStage<TransformBuffer>(device, this));
        }
        
        public string Name { get; set; }
        
        public int VertexCount { get; set; }

        // public override void Draw()
        // {
        //     //todo transform
        //     base.Draw();
        // }

        private void Initialize(IEnumerable<StageBase> stages)
        {
            foreach (var stageBase in stages)
            {
                AddStage(stageBase);
            }
        }
    }
}