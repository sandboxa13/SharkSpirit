using Avalonia;
using Avalonia.Markup.Xaml;

namespace SharkSpirit.Modules.SceneInspector
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
