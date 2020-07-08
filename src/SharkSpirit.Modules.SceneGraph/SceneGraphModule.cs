using System;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using SharkSpirit.Modules.SceneGraph.Views;

namespace SharkSpirit.Modules.SceneGraph
{
    public class SceneGraphModule : IModule
    {
        private readonly IContainer _container;

        public SceneGraphModule(IContainer container)
        {
            _container = container;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _container.RegisterTypeForNavigation<SceneGraphView>("SceneGraph");
        }
    }
}
