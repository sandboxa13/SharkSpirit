using System.Collections.Generic;
using SharkSpirit.RenderFramework.DirectX.Pipeline;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class RenderObject
    {
        private readonly List<BindableBase> _bindables;

        public RenderObject()
        {
            _bindables = new List<BindableBase>();

        }
        
        protected IndexBufferBindable IndexBufferBindable;
        protected IDevice Device;

        protected void AddBindable(BindableBase bindable)
        {
            _bindables.Add(bindable);
        }

        protected void AddIndexBuffer(IndexBufferBindable indexBufferBindable)
        {
            IndexBufferBindable = indexBufferBindable;

            _bindables.Add(indexBufferBindable);
        }
        
        public Matrix World { get; private set; }
        public Matrix ViewProjection { get; private set; }
        public Matrix View { get; private set; }

        public void UpdateWorld(Matrix world)
        {
            World = world;
        }

        public void UpdateViewProjection(Matrix viewProjection)
        {
            ViewProjection = viewProjection;
        }

        public void UpdateView(Matrix view)
        {
            View = view;
        }
        
        public void Draw()
        {
            foreach (var bindable in _bindables)
            {
                bindable.Bind();
            }

            Device.GetDeviceContext().DrawIndexed(IndexBufferBindable.GetCount(), 0, 0);
        }
    }
}