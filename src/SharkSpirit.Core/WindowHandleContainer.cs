using System;

namespace SharkSpirit.Core
{
    public class WindowHandleContainer
    {
        public WindowHandleContainer(IntPtr hwnd)
        {
            WindowHandle = hwnd;
        }
        
        public IntPtr WindowHandle { get; private set; }
    }
}