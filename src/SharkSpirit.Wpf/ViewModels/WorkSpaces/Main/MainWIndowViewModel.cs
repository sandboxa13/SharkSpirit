using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Modules.Core.Resources.Themes.DarkTheme;
using SharkSpirit.Modules.Core.ViewModels;
using Xceed.Wpf.AvalonDock.Themes;

namespace SharkSpirit.Wpf.ViewModels.WorkSpaces.Main
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            AvalonDockCurrentTheme = new AvalonDockDarkTheme();
        }

        [Reactive] public Theme AvalonDockCurrentTheme { get; set; }


        public Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
