using ReactiveUI.Fody.Helpers;
using SharkSpirit.Editor.Core.Services;

namespace SharkSpirit.Editor.Core.ViewModels.Launcher
{
    public class LauncherViewModel : ViewModelBase
    {
        public LauncherViewModel(INavigationService navigationService)
        {
            OpenProjectViewModel = new OpenProjectViewModel();
            CreateProjectViewModel = new CreateProjectViewModel(navigationService);
        }

        [Reactive] public OpenProjectViewModel OpenProjectViewModel { get; set; }
        [Reactive] public CreateProjectViewModel CreateProjectViewModel { get; set; }
    }
}
