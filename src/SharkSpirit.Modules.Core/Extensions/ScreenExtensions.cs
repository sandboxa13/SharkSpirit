using System.Windows;
using System.Windows.Interop;

namespace SharkSpirit.Modules.Core.Extensions
{
    public static class ScreenExtensions
    {
        public static System.Windows.Forms.Screen GetScreen(this Window window)
        {
            return System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(window).Handle);
        }
    }
}
