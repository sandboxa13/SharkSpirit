using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;
using SharpDX.Direct3D;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class Cube : RenderObject
    {
        public Cube(IDevice device, IConfiguration configuration) : base(device)
        {
            float side = 1.0f / 2.0f;

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

            var indices = new ushort[]
            {
                0,2, 1,    2,3,1,
                4,5, 7,    4,7,6,
                8,10, 9,  10,11,9,
                12,13,15, 12,15,14,
                16,17,18, 18,17,19,
                20,23,21, 20,22,23
            };

            AddStage(new VertexBufferStage<SimpleVertex>(device, vertices.ToArray()));
            AddStage(new TextureStage(device));
            AddStage(new SamplerStage(device));
            AddStage(new VertexShaderStage(device, Path.Combine(configuration.PathToShaders, "vertexShader.hlsl")));
            AddStage(new PixelShaderStage(device, Path.Combine(configuration.PathToShaders, "pixelShader.hlsl")));

            AddIndexBufferStage(new IndexBufferStage(device, indices));

            AddStage(new InputLayoutStage(device, Path.Combine(configuration.PathToShaders, "vertexShader.hlsl")));
            AddStage(new TopologyStage(device, PrimitiveTopology.TriangleList));
            AddStage(new TransformConstantBufferStage(device, this));
        }
    }
}