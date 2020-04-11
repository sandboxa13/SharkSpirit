using System.Threading.Tasks;
using SharkSpirit.Modules.Core.ViewModels;

namespace SharkSpirit.Wpf.ViewModels.WorkSpaces.Main
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
