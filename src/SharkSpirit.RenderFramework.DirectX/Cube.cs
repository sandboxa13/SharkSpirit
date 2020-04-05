using System.IO;
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
            var vertices = new[]
            {
                new SimpleVertex(new Vector3(-1.0f, 1.0f, -1.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.5f)),
                new SimpleVertex(new Vector3(1.0f, 1.0f, -1.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.5f)),
                new SimpleVertex(new Vector3(1.0f, 1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.5f)),
                new SimpleVertex(new Vector3(-1.0f, 1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.5f)),
                new SimpleVertex(new Vector3(-1.0f, -1.0f, -1.0f), new Vector4(1.0f, 0.0f, 1.0f, 0.5f)),
                new SimpleVertex(new Vector3(1.0f, -1.0f, -1.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.5f)),
                new SimpleVertex(new Vector3(1.0f, -1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 0.5f)),
                new SimpleVertex(new Vector3(-1.0f, -1.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.5f)),
            };

            AddStage(new VertexShaderStage(device, Path.Combine(configuration.PathToShaders, "vertexShader.hlsl")));

            AddStage(new PixelShaderStage(device, Path.Combine(configuration.PathToShaders, "pixelShader.hlsl")));

            AddStage(new VertexBufferStage<SimpleVertex>(device, vertices));

            var indices = new ushort[]
            {
                3, 1, 0,
                2, 1, 3,

                0, 5, 4,
                1, 5, 0,

                3, 4, 7,
                0, 4, 3,

                1, 6, 5,
                2, 6, 1,

                2, 7, 6,
                3, 7, 2,

                6, 4, 5,
                7, 4, 6,
            };


            AddIndexBufferStage(new IndexBufferStage(device, indices));

            AddStage(new InputLayoutStage(device, Path.Combine(configuration.PathToShaders, "vertexShader.hlsl")));

            AddStage(new TopologyStage(device, PrimitiveTopology.TriangleList));

            AddStage(new TransformConstantBufferStage(device, this));
        }
    }
}