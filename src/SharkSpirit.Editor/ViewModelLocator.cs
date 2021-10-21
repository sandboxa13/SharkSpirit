using SharkSpirit.Editor.Core.ViewModels.Launcher;

namespace SharkSpirit.Editor
{
    public class ViewModelLocator
    {
        public LauncherViewModel LauncherViewModel => new (App.Navigation);
    }
}
