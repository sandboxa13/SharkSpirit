using System;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX;
using SharkSpirit.RenderFramework.DirectX.Avalonia;

namespace SharkSpirit.Engine.Systems
{
    public static class RenderSystemFactory
    {
        public static RenderSystem CreateRenderSystem(IContainer container, IConfiguration configuration)
        {
            return CreateRenderSystemInternal(container, configuration);
        }

        private static RenderSystem CreateRenderSystemInternal(IContainer container, IConfiguration configuration)
        {
            switch (configuration.EngineEditorType)
            {
                case EngineEditorType.Avalonia:
                    return CreateAvaloniaRenderSystem(container);
                case EngineEditorType.Wpf:
                    return CreateWpfRenderSystem(container);
                case EngineEditorType.Default:
                    return CreateDefaultRenderSystem(container);
                default:
                    throw new ArgumentOutOfRangeException(nameof(configuration.EngineEditorType), configuration.EngineEditorType, "This editor type not supported");
            }
        }

        private static RenderSystem CreateDefaultRenderSystem(IContainer container)
        {
            var device = new DefaultDevice(container);
            device.Initialize();
            
            var scene = container.GetService<IScene>();

            return new RenderSystem(device, scene, container);
        }

        private static RenderSystem CreateWpfRenderSystem(IContainer container)
        {
            var device = new WpfDevice(container);
            device.Initialize();

            var scene = container.GetService<IScene>();

            return new RenderSystem(device, scene, container);
        }

        private static RenderSystem CreateAvaloniaRenderSystem(IContainer container)
        {
            var avaloniaInteropHelper = container.GetService<AvaloniaInteropHelper>();
            
            var device = new AvaloniaDevice(container, avaloniaInteropHelper);
            device.Initialize();
            
            var scene = container.GetService<IScene>();

            return new RenderSystem(device, scene, container);
        }
    }
}