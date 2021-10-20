using ReactiveUI.Fody.Helpers;
using SharkSpirit.Editor.Core.ViewModels.Launcher;

namespace SharkSpirit.Editor.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LauncherViewModel = new LauncherViewModel();
        }

        [Reactive] public LauncherViewModel LauncherViewModel { get; set; }
    }
}
