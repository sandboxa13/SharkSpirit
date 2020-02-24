using System.IO;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.Pipeline;
using SharpDX;
using SharpDX.Direct3D;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class Cube : RenderObject
    {
        public Cube(IDevice device, IConfiguration configuration)
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

            AddBindable(new VertexShaderBindable(device, Path.Combine(configuration.PathToShaders, "vertexShader.hlsl")));

            AddBindable(new PixelShaderBindable(device, Path.Combine(configuration.PathToShaders, "pixelShader.hlsl")));

            AddBindable(new VertexBufferBindable<SimpleVertex>(device, vertices));

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


            AddIndexBuffer(new IndexBufferBindable(device, indices));

            AddBindable(new InputLayoutBindable(device, Path.Combine(configuration.PathToShaders, "vertexShader.hlsl")));

            AddBindable(new TopologyBindable(device, PrimitiveTopology.TriangleList));

            AddBindable(new TransformConstantBufferBindable(device, this));
        }
    }
}