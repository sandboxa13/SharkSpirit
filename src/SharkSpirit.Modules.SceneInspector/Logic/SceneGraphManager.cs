using System.Collections.Generic;
using SharkSpirit.Core;
using SharkSpirit.Engine;

namespace SharkSpirit.Modules.SceneInspector.Logic
{
    public class SceneGraphManager
    {
        private readonly IContainer _engineContainer;

        public SceneGraphManager(IContainer engineContainer)
        {
            _engineContainer = engineContainer;
        }

        public IEnumerable<Entity> GetSceneEntities()
        {
            return _engineContainer.GetService<IScene>().Entities;
        }
    }
}
