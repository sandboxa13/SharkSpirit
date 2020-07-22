using System;
using System.IO;
using System.Reflection;
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


            var pathToSrc = Path.GetFullPath( Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\"));
            
            
            var graphicsConfiguration = new Configuration
            {
                Height = 1280,
                Width = 720,
                EngineEditorType = EngineEditorType.Wpf,
                PathToShaders = ((pathToSrc + @"SharkSpirit.Graphics\Shaders")),
                PathToModels = ((pathToSrc + @"SharkSpirit.Graphics\Models")),
                MonitorHeight = Screen.PrimaryScreen.Bounds.Height,
                MonitorWidth = Screen.PrimaryScreen.Bounds.Width
            };

            var engineContainer = container.Resolve<Core.IContainer>();
            engineContainer.AddService(graphicsConfiguration);

            container.RegisterInstance(new Game(engineContainer));
        }
    }
}
