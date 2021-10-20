using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SharkSpirit.Editor.Core.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        [Reactive] public string Name { get; set; }
    }
}
