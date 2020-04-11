using System.Windows.Forms;
using DryIoc;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.AvalonDock;
using IContainer = DryIoc.IContainer;

namespace SharkSpirit.Wpf
{
    public class Bootstrapper
    {
        public void RegisterTypes(IContainer container)
        {
            container.Register<ILayoutAnchorableHelper, LayoutAnchorableHelper>();
            container.Register<MainWindow>();
            container.Register<Core.IContainer, Core.Container>(new SingletonReuse());

            var graphicsConfiguration = new Configuration
            {
                Height = 1280,
                Width = 720,
                EngineEditorType = EngineEditorType.Wpf,
                PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders",
                MonitorHeight = Screen.PrimaryScreen.Bounds.Height,
                MonitorWidth = Screen.PrimaryScreen.Bounds.Width
            };

            var engineContainer = container.Resolve<Core.IContainer>();
            engineContainer.AddService(graphicsConfiguration);

            container.RegisterInstance(new Game(engineContainer));
        }
    }
}
