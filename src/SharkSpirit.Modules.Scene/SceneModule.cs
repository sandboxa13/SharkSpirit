using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using SharkSpirit.Modules.Scene.Views;

namespace SharkSpirit.Modules.Scene
{
    public class SceneModule : IModule
    {
        private readonly IContainer _container;

        public SceneModule(IContainer container)
        {
            _container = container;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _container.RegisterTypeForNavigation<SceneView>("Scene");
        }
    }
}