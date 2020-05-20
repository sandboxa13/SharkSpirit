using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Assimp;
using Assimp.Configs;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using ConstantBuffer = SharkSpirit.Graphics.ConstantBuffer;
using PixelShaderStage = SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages.PixelShaderStage;
using VertexShaderStage = SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages.VertexShaderStage;

namespace SharkSpirit.RenderFramework.DirectX.Primitives.Sphere
{
    public class SpherePrimitiveBuilder : AbstractPrimitiveBuilder
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public Vector3 Position;

            public Vertex(Vector3 position)
            {
                Position = position;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConstantColor
        {
            public Vector4 Color;

            public ConstantColor(Vector4 color)
            {
                Color = color;
            }
        }

        public SpherePrimitiveBuilder(IDevice device, IConfiguration configuration) : base(device, configuration)
        {
        }

        public Sphere Build(Sphere sphere)
        {
            var importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            var model = importer.ImportFile("C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\sponza\\Lampochka_Kolyana (2).obj", PostProcessPreset.TargetRealTimeMaximumQuality);

            var vertices = new List<Vertex>();
            var indices = new List<ushort>();

            foreach (var modelMesh in model.Meshes)
            {
                for (var i = 0; i < modelMesh.VertexCount; i++)
                {
                    vertices.Add(new Vertex(
                        new Vector3(modelMesh.Vertices[i].X, modelMesh.Vertices[i].Y, modelMesh.Vertices[i].Z)));
                }

                for (var i = 0; i < modelMesh.FaceCount; i++)
                {
                    var face = modelMesh.Faces[i];

                    foreach (var faceIndex in face.Indices)
                    {
                        indices.Add((ushort)faceIndex);
                    }
                }
            }

            AddVertexBufferStage(sphere, vertices);
            AddIndexBufferStage(sphere, indices);
            AddVertexShaderStage(sphere);
            AddPixelShaderStage(sphere);
            AddPixelConstantBufferStage(sphere);
            AddInputLayoutStage(sphere);
            AddTopologyStage(sphere);
            AddTransformConstantBufferStage(sphere);

            return sphere;
        }

        
        private void AddVertexBufferStage(RenderObject renderObject, List<Vertex> vertices)
        {
            renderObject.AddStage(new VertexBufferStage<Vertex>(Device, vertices.ToArray()));
        }

        private void AddPixelConstantBufferStage(RenderObject renderObject)
        {
            renderObject.AddStage(new PixelConstantBufferStage<LightCBuf>(Device, renderObject));
        }

        private void AddIndexBufferStage(RenderObject renderObject, List<ushort> indices)
        {
            renderObject.AddIndexBufferStage(new IndexBufferStage(Device, indices.ToArray()));
        }

        protected override void AddVertexBufferStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddTextureStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddSamplerStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddVertexShaderStage(RenderObject renderObject)
        {
            renderObject.AddStage(new VertexShaderStage(Device, Path.Combine(Configuration.PathToShaders, "SolidVS.hlsl")));
        }

        protected override void AddPixelShaderStage(RenderObject renderObject)
        {
            renderObject.AddStage(new PixelShaderStage(Device, Path.Combine(Configuration.PathToShaders, "SolidPS.hlsl")));
        }

        protected override void AddIndexBufferStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddInputLayoutStage(RenderObject renderObject)
        {
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(Path.Combine(Configuration.PathToShaders, "SolidVS.hlsl"), "VS", "vs_4_0", ShaderFlags.Debug);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            var inputLayout = new InputLayout(Device.GetDevice(), signature, new[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
            });

            renderObject.AddStage(new InputLayoutStage(Device, inputLayout));
        }

        protected override void AddTopologyStage(RenderObject renderObject)
        {
            renderObject.AddStage(new TopologyStage(Device, PrimitiveTopology.TriangleList));
        }

        protected override void AddTransformConstantBufferStage(RenderObject renderObject)
        {
            renderObject.AddStage(new TransformConstantBufferStage<ConstantBuffer>(Device, renderObject));

        }
    }
}
