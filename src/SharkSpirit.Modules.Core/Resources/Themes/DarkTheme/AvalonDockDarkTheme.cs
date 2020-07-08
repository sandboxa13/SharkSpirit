using System;
using Xceed.Wpf.AvalonDock.Themes;

namespace SharkSpirit.Modules.Core.Resources.Themes.DarkTheme
{
    public class AvalonDockDarkTheme : Theme
    {
        public override Uri GetResourceUri()
        {
            return new Uri(
                "pack://application:,,,/SharkSpirit.Modules.Core;component/Resources/Themes/DarkTheme/AvalonDockDarkTheme.xaml",
                UriKind.Absolute);
        }
    }
}
