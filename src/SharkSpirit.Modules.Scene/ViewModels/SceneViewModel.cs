using System;
using System.Threading.Tasks;
using DryIoc;
using Prism.Regions;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharpDX;
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
            var container = _container.Resolve<SharkSpirit.Core.IContainer>();

            if (_game != null)
                return;

            _game = _container.Resolve<Game>();

            Task.Run(async () => { await _game.Scene.AddEntityAsync(new Entity(new Vector3(0, 0, 0), container, $"Point Light"));});
            Task.Run(async () => { await _game.Scene.LoadModelAsync("Sponza\\sponza.obj",1.0f / 40.0f);});
            Task.Run(async () => { await _game.Scene.LoadModelAsync("nano_textured\\Nanosuit.obj", 3f, false);});
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
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
