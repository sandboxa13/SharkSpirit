using SharkSpirit.Core;
using SharpDX.DirectInput;

namespace SharkSpirit.InputEngine.Mouse
{
    public class MouseDevice : IMouseDevice
    {
        private MouseState _mouseState;
        private readonly SharpDX.DirectInput.Mouse _mouse;

        public MouseDevice(DirectInput directInput)
        {
            _mouse = new SharpDX.DirectInput.Mouse(directInput);
            _mouseState = new MouseState();

            _mouse.Acquire();
        }

        public void Update() => _mouse.GetCurrentState(ref _mouseState);
        public bool LMouseDown() => _mouseState.Buttons[0];
        public bool RMouseDown() => _mouseState.Buttons[1];
        public float MouseX() => System.Windows.Forms.Cursor.Position.X;
        public float MouseY() => System.Windows.Forms.Cursor.Position.Y;
        public float RawMouseX() => _mouseState.X;
        public float RawMouseY() => _mouseState.Y;

        public ScrollDirection ScrollDirection()
        {
            switch (_mouseState.Z)
            {
                case 0:
                    return Core.ScrollDirection.None;
                default:
                {
                    if (_mouseState.Z > 0)
                    {
                        return Core.ScrollDirection.Forward;
                    }

                    if (_mouseState.Z < 0)
                    {
                        return Core.ScrollDirection.Back;
                    }

                    break;
                }
            }
            return Core.ScrollDirection.None;
        }
    }

    public interface IMouseDevice
    {
        void Update();

        bool LMouseDown();
        bool RMouseDown();

        float MouseX();
        float MouseY();

        float RawMouseX();
        float RawMouseY();

        ScrollDirection ScrollDirection();
    }
}
