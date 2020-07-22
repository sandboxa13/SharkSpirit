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
        private Node _rootNode;

        public Model(IDevice device, IConfiguration configuration, string modelName) : base(device, MeshType.None)
        {
            Meshes = new List<Mesh>();

            Initialize(modelName, device, configuration);
        }
        
        public readonly List<Mesh> Meshes;

        public override void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }

            _rootNode.Draw();
        }

        private void Initialize(string modelName, IDevice device, IConfiguration configuration)
        {
            var importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            var scene = importer.ImportFile(modelName, PostProcessPreset.TargetRealTimeMaximumQuality);

            foreach (var sceneMesh in scene.Meshes)
            {
                Meshes.Add(ParseMesh(sceneMesh, device, configuration));
            }

            _rootNode = ParseNode(scene.RootNode);
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

        private Mesh ParseMesh(Assimp.Mesh modelMesh, IDevice device, IConfiguration configuration)
        {
            var vertices = new List<ModelLoading.Model.Vertex>();
            var indices = new List<ushort>();

            for (var i = 0; i < modelMesh.VertexCount; i++)
            {
                //todo multiply by size
                vertices.Add(new ModelLoading.Model.Vertex(
                    new Vector3(modelMesh.Vertices[i].X, modelMesh.Vertices[i].Y, modelMesh.Vertices[i].Z),
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


            var stages = new List<StageBase>();

            stages.Add(new VertexBufferStage<ModelLoading.Model.Vertex>(device, vertices.ToArray()));
            stages.Add(new IndexBufferStage(device, indices.ToArray()));
            
            stages.Add(new VertexShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongVS.hlsl")));
            stages.Add(new PixelShaderStage(device, Path.Combine(configuration.PathToShaders, "PhongPS.hlsl")));
            
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(Path.Combine(configuration.PathToShaders, "PhongVS.hlsl"), "VS", "vs_4_0", ShaderFlags.Debug);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            var inputLayout = new InputLayout(device.GetDevice(), signature, new[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("Normal", 0, Format.R32G32B32_Float, 12, 0),
            });
            stages.Add(new InputLayoutStage(device, inputLayout));

            var ocb = new ObjectCBuf
            {
                MaterialColor = new Vector3(1, 0, 0),
                SpecularPower = 30.0f,
                SpecularIntensity = 0.6f
            };

            stages.Add(new PixelConstantBufferStage<ObjectCBuf>(device, ocb, 1));

            stages.Add(new TopologyStage(device, PrimitiveTopology.TriangleList));
            
            return new Mesh(device, stages);
        }
    }
}