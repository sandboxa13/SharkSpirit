using System.Windows;
using SharkSpirit.Editor.Core.ViewModels;

namespace SharkSpirit.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();

            InitializeComponent();
        }
    }
}
