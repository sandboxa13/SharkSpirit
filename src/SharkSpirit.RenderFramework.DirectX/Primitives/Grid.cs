using System.Collections.Generic;
using System.Linq;
using SharkSpirit.Graphics;

namespace SharkSpirit.RenderFramework.DirectX.Primitives
{
    public class Grid : RenderObject
    {
        public Grid(IDevice device, float width, float depth, int m, int n) : base(device, MeshType.Grid)
        {
          
        }
    }

    public class MeshData
    {
        public List<SimpleVertex> Vertices;
        public List<int> Indices32;

        public List<int> GetIndices16()
        {
            if (!mIndices16.Any())
            {
                for (var i = 0; i < Indices32.Count; ++i)
                    mIndices16[i] = (Indices32[i]);
            }

            return mIndices16;
        }

        private List<int> mIndices16;
    };
}
