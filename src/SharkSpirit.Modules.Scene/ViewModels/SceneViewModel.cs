using System;
using System.Windows.Forms;
using Prism.Regions;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharpDX;
using Application = System.Windows.Application;
using Container = SharkSpirit.Core.Container;
using IContainer = DryIoc.IContainer;

namespace SharkSpirit.Modules.Scene.ViewModels
{
    public class SceneViewModel : ViewModelBase, INavigationAware
    {
        private readonly IContainer _container;
        private Game _game;

        public SceneViewModel(IContainer container)
        {
            _container = container;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var container = new Container();

            var graphicsConfiguration = new SharkSpirit.Core.Configuration
            {
                Height = (float)Application.Current.MainWindow.Height,
                Width = (float)Application.Current.MainWindow.Width,
                EngineEditorType = EngineEditorType.Wpf,
                PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders",
                MonitorHeight = Screen.PrimaryScreen.Bounds.Height,
                MonitorWidth = Screen.PrimaryScreen.Bounds.Width
            };

            container.AddService(graphicsConfiguration);

            if (_game != null)
                return;

            _game = new Game(container);

            var tmp = 0;

            for (var i = 0; i < 40; i++)
            {
                _game.Scene.AddEntity(new Entity(new Vector3(tmp, 0, 0), container));

                tmp += 5;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        public void SetWindowHandle(IntPtr windowHandle)
        {
            var windowHandleContainer = new WindowHandleContainer(windowHandle);
            _game.GetContainer().AddService(windowHandleContainer);
        }

        public void OnRender(IntPtr resourcePointer, bool isNewSurface)
        {
            if (isNewSurface)
            {
                _game.Reinitialize(resourcePointer);
            }

            _game.Update();
        }

        public SharkSpirit.Core.IContainer GetContainer()
        {
            return _game.GetContainer();
        }
    }
}
