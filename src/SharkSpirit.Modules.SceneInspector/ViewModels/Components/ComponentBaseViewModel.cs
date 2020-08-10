using SharkSpirit.Core;
using SharkSpirit.Modules.Core.ViewModels;

namespace SharkSpirit.Modules.SceneInspector.ViewModels.Components
{
    public class ComponentBaseViewModel : ViewModelBase
    {
        public ComponentBaseViewModel(ComponentBase componentBase)
        {
            Name = componentBase.Name;
            Component = componentBase;
        }
        
        public ComponentBase Component { get; }
        
        // todo logic for apply or rollback changes 
        public virtual void Refresh()
        {
            
        }
    }
}