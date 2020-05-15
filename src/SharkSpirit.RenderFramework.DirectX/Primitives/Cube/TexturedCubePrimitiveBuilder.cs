using System.Collections.Generic;
using System.IO;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using PixelShaderStage = SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages.PixelShaderStage;
using VertexShaderStage = SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages.VertexShaderStage;

namespace SharkSpirit.RenderFramework.DirectX.Primitives
{
    public class TexturedCubePrimitiveBuilder : AbstractPrimitiveBuilder
    {
        public TexturedCubePrimitiveBuilder(
            IDevice device, 
            IConfiguration configuration) : base(device, configuration)
        {
        }

        public Cube Build(Cube cube)
        {
            AddVertexBufferStage(cube);
            AddTextureStage(cube);
            AddSamplerStage(cube);
            AddVertexShaderStage(cube);
            AddPixelShaderStage(cube);
            AddIndexBufferStage(cube);
            AddInputLayoutStage(cube);
            AddTopologyStage(cube);
            AddTransformConstantBufferStage(cube);

            return cube;
        }

        #region Stages

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
            renderObject.AddStage(new TextureStage(Device, "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders\\1_store.png"));
        }
        protected override void AddSamplerStage(RenderObject renderObject)
        {
            renderObject.AddStage(new SamplerStage(Device));
        }
        protected override void AddVertexShaderStage(RenderObject renderObject)
        {
            renderObject.AddStage(new VertexShaderStage(Device, Path.Combine(Configuration.PathToShaders, "vertexShader.hlsl")));
        }
        protected override void AddPixelShaderStage(RenderObject renderObject)
        {
            renderObject.AddStage(new PixelShaderStage(Device, Path.Combine(Configuration.PathToShaders, "pixelShader.hlsl")));
        }
        protected override void AddIndexBufferStage(RenderObject renderObject)
        {
            var indices = new ushort[]
            {
                0,2, 1,    2,3,1,
                4,5, 7,    4,7,6,
                8,10, 9,  10,11,9,
                12,13,15, 12,15,14,
                16,17,18, 18,17,19,
                20,23,21, 20,22,23
            };

            renderObject.AddIndexBufferStage(new IndexBufferStage(Device, indices));
        }
        protected override void AddInputLayoutStage(RenderObject renderObject)
        {
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(Path.Combine(Configuration.PathToShaders, "vertexShader.hlsl"), "VS", "vs_4_0", ShaderFlags.Debug);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            var inputLayout = new InputLayout(Device.GetDevice(), signature, new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0),
            });

            renderObject.AddStage(new InputLayoutStage(Device, inputLayout));
        }
        protected override void AddTopologyStage(RenderObject renderObject)
        {
            renderObject.AddStage(new TopologyStage(Device, PrimitiveTopology.TriangleList));
        }
        protected override void AddTransformConstantBufferStage(RenderObject renderObject)
        {
            renderObject.AddStage(new TransformConstantBufferStage(Device, renderObject));
        }

        #endregion
    }
}