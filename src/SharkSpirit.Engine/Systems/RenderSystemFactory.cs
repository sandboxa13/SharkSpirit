using System;
using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX;

namespace SharkSpirit.Engine.Systems
{
    public static class RenderSystemFactory
    {
        public static RenderSystem CreateRenderSystem(IContainer container)
        {
            return CreateRenderSystemInternal(container);
        }

        private static RenderSystem CreateRenderSystemInternal(IContainer container)
        {
            var config = container.Resolve<IConfiguration>();

            switch (config.EngineEditorType)
            {
                case EngineEditorType.Avalonia:
                    return CreateAvaloniaRenderSystem(container);
                case EngineEditorType.Wpf:
                    return CreateWpfRenderSystem(container);
                case EngineEditorType.Default:
                    return CreateDefaultRenderSystem(container);
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.EngineEditorType), config.EngineEditorType, "This editor type not supported");
            }
        }

        private static RenderSystem CreateDefaultRenderSystem(IContainer container)
        {
            var device = new DefaultDevice(container);

            return new RenderSystem(device);
        }

        private static RenderSystem CreateWpfRenderSystem(IContainer container)
        {
            var device = new WpfDevice(container);

            return new RenderSystem(device);
        }

        private static RenderSystem CreateAvaloniaRenderSystem(IContainer container)
        {
            var device = new AvaloniaDevice(container);

            return new RenderSystem(device);
        }
    }
}