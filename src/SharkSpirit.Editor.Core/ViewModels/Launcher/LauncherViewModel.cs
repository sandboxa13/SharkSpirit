using ReactiveUI.Fody.Helpers;

namespace SharkSpirit.Editor.Core.ViewModels.Launcher
{
    public class LauncherViewModel : ViewModelBase
    {
        public LauncherViewModel()
        {
            OpenProjectViewModel = new OpenProjectViewModel();
            CreateProjectViewModel = new CreateProjectViewModel();
        }

        [Reactive] public OpenProjectViewModel OpenProjectViewModel { get; set; }
        [Reactive] public CreateProjectViewModel CreateProjectViewModel { get; set; }
    }
}
