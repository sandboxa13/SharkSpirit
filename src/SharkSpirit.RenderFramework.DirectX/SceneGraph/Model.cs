using System.Collections.Generic;
using System.IO;
using Assimp;
using Assimp.Configs;
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

namespace SharkSpirit.RenderFramework.DirectX.SceneGraph
{
    public class Model : RenderObject
    {
        private readonly float _scale;

        public Model(IDevice device, IConfiguration configuration, string modelName, float scale = 1f) : base(device)
        {
            _scale = scale;
            Meshes = new List<Mesh>();

            Initialize(modelName, device, configuration, modelName);
        }
        
        public readonly List<Mesh> Meshes;
        public Node RootNode;

        public override void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }

            RootNode.Draw();
        }

        private void Initialize(string modelName, IDevice device, IConfiguration configuration, string path)
        {
            var importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            var scene = importer.ImportFile(modelName, PostProcessPreset.TargetRealTimeMaximumQuality);

            foreach (var sceneMesh in scene.Meshes)
            {
                Meshes.Add(ParseMesh(scene, sceneMesh, device, configuration, path));
            }

            RootNode = ParseNode(scene.RootNode);
        }

        private Node ParseNode(Assimp.Node node)
        {
            var currentMeshes = new List<Mesh>();

            for (var i = 0; i < node.MeshCount; i++)
            {
                var meshIndex = node.MeshIndices[i];

                var mesh = Meshes[meshIndex];

                currentMeshes.Add(mesh);
            }

            var newNode = new Node(currentMeshes, node.Name);

            foreach (var child in node.Children)
            {
                newNode.AddChild(ParseNode(child));
            }

            return newNode;
        }

        private Mesh ParseMesh(Scene scene, Assimp.Mesh modelMesh, IDevice device, IConfiguration configuration, string path)
        {
            var vertices = new List<Vertex>();
            var indices = new List<ushort>();

            for (var i = 0; i < modelMesh.VertexCount; i++)
            {
                var texC = modelMesh.TextureCoordinateChannels[0][i];

                vertices.Add(new Vertex(
                    new Vector3(modelMesh.Vertices[i].X * _scale, modelMesh.Vertices[i].Y * _scale, modelMesh.Vertices[i].Z * _scale),
                    new Vector3(modelMesh.Normals[i].X, modelMesh.Normals[i].Y, modelMesh.Normals[i].Z),
                    new Vector2(texC.X, texC.Y)));
            }

            for (var i = 0; i < modelMesh.FaceCount; i++)
            {
                var face = modelMesh.Faces[i];

                foreach (var faceIndex in face.Indices)
                {
                    indices.Add((ushort) faceIndex);
                }
            }

            var stages = new List<StageBase>();

            var hasSpecular = false;
            
            if (modelMesh.MaterialIndex >= 0)
            {
                var material = scene.Materials[modelMesh.MaterialIndex];
                
                material.GetMaterialTexture(TextureType.Diffuse, 0, out var diffuse);
                var diffPath = diffuse.FilePath;
                
                stages.Add(new TextureStage(device, Path.Combine(path + @"\..", diffPath)));

                material.GetMaterialTexture(TextureType.Specular, 0, out var specular);
                
                if (!string.IsNullOrEmpty(specular.FilePath))
                {
                    var specPath = specular.FilePath;
                
                    stages.Add(new TextureStage(device, Path.Combine(path + @"\..", specPath), 1));
            
                    hasSpecular = true;
                }
                
                stages.Add(new SamplerStage(device));
            }


            stages.Add(new VertexBufferStage<Vertex>(device, vertices.ToArray()));
            stages.Add(new IndexBufferStage(device, indices.ToArray()));
            
            stages.Add(new VertexShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongVS.hlsl")));

            if (hasSpecular)
            {
                stages.Add(new PixelShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongSpecularPS.hlsl")));
            }
            else
            {
                stages.Add(new PixelShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongPS.hlsl")));
            }
            
            
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(Path.Combine(configuration.PathToShaders, "PhongVS.hlsl"), "VS", "vs_4_0", ShaderFlags.Debug);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            var inputLayout = new InputLayout(device.GetDevice(), signature, new[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("Normal", 0, Format.R32G32B32_Float, 12, 0),
                new InputElement("Texcoord", 0, Format.R32G32_Float, 24, 0),

            });
            stages.Add(new InputLayoutStage(device, inputLayout));

            var ocb = new ObjectCBuf
            {
                SpecularPower = 50.0f,
                SpecularIntensity = 1.6f
            };
            
            stages.Add(new PixelConstantBufferStage<ObjectCBuf>(device, ocb, this, 1));

            stages.Add(new TopologyStage(device, PrimitiveTopology.TriangleList));
            
            return new Mesh(device, stages, modelMesh.Name, vertices.Count);
        }
    }
}