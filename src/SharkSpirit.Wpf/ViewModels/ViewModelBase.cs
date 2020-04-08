using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SharkSpirit.Wpf.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        [Reactive] public string Name { get; set; }
    }
}
