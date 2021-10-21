using System.Reactive.Concurrency;
using System.Windows;
using ReactiveUI;
using SharkSpirit.Editor.Core.ViewModels;
using SharkSpirit.Editor.Services;
using SharkSpirit.Editor.Views.Launcher;

namespace SharkSpirit.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static NavigationService Navigation;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            mainWindow.Show();

            Navigation = new NavigationService(mainWindow.Frame);

            Navigation.Navigate<LauncherView>();
        }
    }
}
