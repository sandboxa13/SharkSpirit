using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Editor.Core.Services;

namespace SharkSpirit.Editor.Core.ViewModels.Launcher
{
    public class CreateProjectViewModel : ViewModelBase
    {
        public CreateProjectViewModel(INavigationService navigationService)
        {
            CreateCommand = ReactiveCommand.Create(() =>
            {
                navigationService.Navigate("EngineView");
            });
        }

        [Reactive] public string PathToProject { get; set; }
        [Reactive] public string ProjectName { get; set; }

        public ReactiveCommand<Unit, Unit> CreateCommand { get; set; }
    }
}
