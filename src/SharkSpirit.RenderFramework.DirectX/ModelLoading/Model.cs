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
using PixelShaderStage = SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages.PixelShaderStage;
using VertexShaderStage = SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages.VertexShaderStage;

namespace SharkSpirit.RenderFramework.DirectX.ModelLoading
{
    // A container for the meshes loaded from the file
    public class Model : RenderObject
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public Vector3 Position;
            public Vector3 N;
            
            public Vertex(Vector3 position, Vector3 n)
            {
                Position = position;
                N = n;
            }
        }
        
        public Model(IDevice device, IConfiguration configuration) : base(device)
        {
            var importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            
            var scene = importer.ImportFile("C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\sponza\\suzanne.obj", PostProcessPreset.TargetRealTimeMaximumQuality);

            var vertices = new List<Vertex>();
            var indices = new List<ushort>();
            
            foreach (var modelMesh in scene.Meshes)
            {
                for (var i = 0; i < modelMesh.VertexCount; i++)
                {
                    vertices.Add(new Vertex(
                        new Vector3(modelMesh.Vertices[i].X,modelMesh.Vertices[i].Y,modelMesh.Vertices[i].Z ), 
                        new Vector3(modelMesh.Normals[i].X, modelMesh.Normals[i].Y, modelMesh.Normals[i].Z)));
                }

                for (var i = 0; i < modelMesh.FaceCount; i++)
                {
                    var face = modelMesh.Faces[i];

                    foreach (var faceIndex in face.Indices)
                    {
                        indices.Add((ushort) faceIndex);
                    }
                }
            }
            
            AddStage(new VertexBufferStage<Vertex>(device, vertices.ToArray()));
            AddIndexBufferStage(new IndexBufferStage(device, indices.ToArray()));
            
            AddStage(new VertexShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongVS.hlsl")));
            AddStage(new PixelShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongPS.hlsl")));
            
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(Path.Combine(configuration.PathToShaders, "PhongVS.hlsl"), "VS", "vs_4_0", ShaderFlags.Debug);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            var inputLayout = new InputLayout(device.GetDevice(), signature, new[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("Normal", 0, Format.R32G32B32_Float, 12, 0),
            });
            AddStage(new InputLayoutStage(device, inputLayout));

            AddStage(new TopologyStage(device, PrimitiveTopology.TriangleList));
            AddStage(new TransformConstantBufferStage(device, this));
        }
    }
}
