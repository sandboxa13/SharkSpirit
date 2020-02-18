using System.Collections.Generic;
using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine
{
    public class Scene : ComponentBase
    {
        public Scene(IContainer container)
        {
            Initialize(container);
        }

        public RenderSystem RenderSystem { get; private set; }

        public List<Entity> Entities { get; private set; }

        private void Initialize(IContainer container)
        {
            RenderSystem.CreateRenderSystem(container);
        }
    }
}
