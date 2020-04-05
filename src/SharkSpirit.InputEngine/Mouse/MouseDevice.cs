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

        public void Update()
        {
            _mouse.GetCurrentState(ref _mouseState);
        }

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

        ScrollDirection ScrollDirection();
    }
}
