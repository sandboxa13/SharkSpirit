using System.Collections.ObjectModel;
using System.Linq;
using SharkSpirit.Editor.Core.Classes.GameProject;

namespace SharkSpirit.Editor.Core.ViewModels.Launcher
{
    public class OpenProjectViewModel : ViewModelBase
    {
        public OpenProjectViewModel()
        {
            ProjectTemplateViewModels = new ReadOnlyCollection<ProjectTemplateViewModel>(
                BaseProjectTemplatesProvider
                    .GetDefaultProjectTemplates()
                    .Select(template => new ProjectTemplateViewModel(template))
                    .ToArray());
        }

        public ReadOnlyCollection<ProjectTemplateViewModel> ProjectTemplateViewModels { get; private set; }
    }
}
