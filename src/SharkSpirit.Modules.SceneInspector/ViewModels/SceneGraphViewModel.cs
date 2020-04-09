using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneGraphViewModel : ViewModelBase
    {
        public SceneGraphViewModel(SceneGraphManager sceneGraphManager)
        {
        }

       
    }

    public class SceneGraphEntityViewModel : ViewModelBase
    {
        public SceneGraphEntityViewModel(Entity entity)
        {
            Name = entity.Name;
        }
    }
}
