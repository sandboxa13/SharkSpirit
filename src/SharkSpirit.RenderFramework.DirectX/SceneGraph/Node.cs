using System.Collections.Generic;

namespace SharkSpirit.RenderFramework.DirectX.SceneGraph
{
    public class Node
    {
        private readonly List<Node> _childs;
        private readonly List<Mesh> _meshes;

        public Node(IEnumerable<Mesh> meshes, string nodeName)
        {
            _childs = new List<Node>();
            _meshes = new List<Mesh>(meshes);

            Name = nodeName;
        }

        public string Name { get; set; }

        public void Draw()
        {
            foreach (var mesh in _meshes)
            {
                mesh.Draw();
            }

            foreach (var child in _childs)
            {
                child.Draw();
            }
        }

        public void AddChild(Node child)
        {
            _childs.Add(child);
        }
    }
}