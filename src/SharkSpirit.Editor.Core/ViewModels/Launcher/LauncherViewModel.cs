using ReactiveUI.Fody.Helpers;
using SharkSpirit.Editor.ViewModels;

namespace SharkSpirit.Editor.Core.ViewModels.Launcher
{
    public class LauncherViewModel : ViewModelBase
    {
        [Reactive] public OpenProjectViewModel OpenProjectViewModel { get; set; }
        [Reactive] public CreateProjectViewModel CreateProjectViewModel { get; set; }
    }
}
