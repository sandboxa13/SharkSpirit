using SharkSpirit.Editor.Core.Utilities;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SharkSpirit.Editor.Views.Engine.RenderSurface
{
    internal class RenderSurfaceView : HwndHost
    {
        private int _width = 1280;
        private int _height = 720;

        public int SurfaceID = ID.InvalidID;

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            throw new System.NotImplementedException();
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            throw new System.NotImplementedException();
        }
    }
}
