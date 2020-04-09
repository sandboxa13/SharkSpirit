using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SharkSpirit.Modules.Core.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        [Reactive] public string Name { get; set; }
    }
}
