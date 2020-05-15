using System.Collections.Generic;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.Primitives
{
    public class CubePrimitiveBuilder : AbstractPrimitiveBuilder
    {
        public CubePrimitiveBuilder(IDevice device, IConfiguration configuration) : base(device, configuration)
        {
        }

        public RenderObject Build(Cube cube)
        {
            return cube;
        }

        protected override void AddVertexBufferStage(RenderObject renderObject)
        {
            var side = 1.0f / 2.0f;

            var vertices = new List<SimpleVertex>
            {
                new SimpleVertex(new Vector4(-side,-side,-side, 1.0f), new Vector2(0.0f,0.0f)),
                new SimpleVertex(new Vector4(side,-side,-side, 1.0f),  new Vector2(1.0f,0.0f )),
                new SimpleVertex(new Vector4(-side,side,-side, 1.0f),  new Vector2( 0.0f,1.0f)),
                new SimpleVertex(new Vector4(side,side,-side, 1.0f),   new Vector2(1.0f,1.0f)),


                new SimpleVertex(new Vector4( -side,-side,side, 1.0f), new Vector2(0.0f,0.0f)),
                new SimpleVertex(new Vector4(side,-side,side, 1.0f),   new Vector2(1.0f,0.0f )),
                new SimpleVertex(new Vector4(-side,side,side, 1.0f),   new Vector2( 0.0f,1.0f)),
                new SimpleVertex(new Vector4( side,side,side, 1.0f),   new Vector2(1.0f,1.0f)),


                new SimpleVertex(new Vector4( -side,-side,-side, 1.0f), new Vector2(0.0f,0.0f)),
                new SimpleVertex(new Vector4(-side,side,-side, 1.0f),   new Vector2(1.0f,0.0f )),
                new SimpleVertex(new Vector4(-side,-side,side, 1.0f),   new Vector2( 0.0f,1.0f)),
                new SimpleVertex(new Vector4( -side,side,side, 1.0f),   new Vector2(1.0f,1.0f)),


                new SimpleVertex(new Vector4( side,-side,-side, 1.0f), new Vector2(0.0f,0.0f)),
                new SimpleVertex(new Vector4(side,side,-side, 1.0f),   new Vector2(1.0f,0.0f )),
                new SimpleVertex(new Vector4(side,-side,side, 1.0f),   new Vector2( 0.0f,1.0f)),
                new SimpleVertex(new Vector4( side,side,side, 1.0f),   new Vector2(1.0f,1.0f)),


                new SimpleVertex(new Vector4( -side,-side,-side, 1.0f), new Vector2(0.0f,0.0f)),
                new SimpleVertex(new Vector4(side,-side,-side, 1.0f),   new Vector2(1.0f,0.0f )),
                new SimpleVertex(new Vector4(-side,-side,side, 1.0f),   new Vector2( 0.0f,1.0f)),
                new SimpleVertex(new Vector4( side,-side,side, 1.0f),   new Vector2(1.0f,1.0f)),


                new SimpleVertex(new Vector4( -side,side,-side, 1.0f), new Vector2(0.0f,0.0f)),
                new SimpleVertex(new Vector4(side,side,-side, 1.0f),   new Vector2(1.0f,0.0f )),
                new SimpleVertex(new Vector4(-side,side,side, 1.0f),   new Vector2( 0.0f,1.0f)),
                new SimpleVertex(new Vector4( side,side,side, 1.0f),   new Vector2(1.0f,1.0f)),
            };

            renderObject.AddStage(new VertexBufferStage<SimpleVertex>(Device, vertices.ToArray()));
        }

        protected override void AddTextureStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddSamplerStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddVertexShaderStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddPixelShaderStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddIndexBufferStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddInputLayoutStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddTopologyStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddTransformConstantBufferStage(RenderObject renderObject)
        {
            throw new System.NotImplementedException();
        }
    }
}