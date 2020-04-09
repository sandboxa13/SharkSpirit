using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using SharkSpirit.Modules.SceneInspector.Views;

namespace SharkSpirit.Modules.SceneInspector
{
    public class SceneInspectorModule : IModule
    {
        private readonly IContainer _container;

        public SceneInspectorModule(IContainer container)
        {
            _container = container;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _container.RegisterTypeForNavigation<SceneInspectorView>("SceneInspector");
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}
