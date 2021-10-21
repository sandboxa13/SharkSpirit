using ReactiveUI.Fody.Helpers;
using SharkSpirit.Editor.Core.Services;
using SharkSpirit.Editor.Core.ViewModels.Launcher;

namespace SharkSpirit.Editor.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(INavigationService navigationService)
        {
            LauncherViewModel = new LauncherViewModel(navigationService);
        }

        [Reactive] public LauncherViewModel LauncherViewModel { get; set; }
    }
}
