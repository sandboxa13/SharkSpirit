using SharkSpirit.Editor.Core.Classes.GameProject;

namespace SharkSpirit.Editor.Core.ViewModels.Launcher
{
    public class ProjectTemplateViewModel : ViewModelBase
    {
        public ProjectTemplateViewModel(ProjectTemplate projectTemplate)
        {
            Name = projectTemplate.Name;
        }
    }
}
