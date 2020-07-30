using System.Collections.Generic;

namespace SharkSpirit.RenderFramework.DirectX.SceneGraph
{
    public class Node
    {
        public Node(IEnumerable<Mesh> meshes, string nodeName)
        {
            Childs = new List<Node>();
            Meshes = new List<Mesh>(meshes);

            Name = nodeName;
        }

        public string Name { get; set; }
        public readonly List<Node> Childs;
        public readonly List<Mesh> Meshes;
        
        public void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }

            foreach (var child in Childs)
            {
                child.Draw();
            }
        }

        public void AddChild(Node child)
        {
            Childs.Add(child);
        }
    }
}