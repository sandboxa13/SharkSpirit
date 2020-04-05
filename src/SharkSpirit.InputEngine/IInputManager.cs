using SharkSpirit.Core;
using SharkSpirit.InputEngine.Keyboard;
using SharkSpirit.InputEngine.Mouse;
using SharpDX.DirectInput;

namespace SharkSpirit.InputEngine
{
    public interface IInputManager
    {
        void Update();
        bool IsPressed(Key key);
        ScrollDirection ScrollDirection();
    }

    public class InputManager : IInputManager
    {
        private readonly DirectInput _directInput;

        public InputManager(IContainer container)
        {
            _directInput = new DirectInput();

            InitializeDevices();
        }
        public IKeyBoardDevice KeyBoardDevice { get; private set; }
        public IMouseDevice MouseDevice { get; private set; }

        public void Update()
        {
            KeyBoardDevice.Update();
            MouseDevice.Update();
        }

        public bool IsPressed(Key key)
        {
            return KeyBoardDevice.IsPressed(key);
        }

        public ScrollDirection ScrollDirection()
        {
            return MouseDevice.ScrollDirection();
        }

        private void InitializeDevices()
        {
            KeyBoardDevice = new KeyboardDevice(_directInput);
            MouseDevice = new MouseDevice(_directInput);
        }
    }
}
