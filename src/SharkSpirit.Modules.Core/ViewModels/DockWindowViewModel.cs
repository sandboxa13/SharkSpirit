using System.Windows.Input;
using ReactiveUI.Fody.Helpers;

namespace SharkSpirit.Modules.Core.ViewModels
{
    public class DockWindowViewModel : ViewModelBase
    {
        public ICommand CloseCommand { get; protected set; }

        [Reactive] public string Title { get; protected set; }

        [Reactive] public bool IsClosed { get; protected set; }

        [Reactive] public bool CanClose{ get; protected set; }
    }
}
