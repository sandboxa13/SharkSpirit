using SharpDX.DirectInput;

namespace SharkSpirit.InputEngine.Keyboard
{
    public class KeyboardDevice : IKeyBoardDevice
    {
        private KeyboardState _keyboardState;
        private readonly SharpDX.DirectInput.Keyboard _keyboard;

        public KeyboardDevice(DirectInput directInput)
        {
            _keyboard = new SharpDX.DirectInput.Keyboard(directInput);
            _keyboard.Properties.AxisMode = DeviceAxisMode.Relative;
            _keyboard.Properties.BufferSize = 128;

            _keyboardState = new KeyboardState();
            _keyboard.Acquire();
        }

        public void Update()
        {
            _keyboard.GetCurrentState(ref _keyboardState);
        }

        public bool IsPressed(Key key)
        {
            return _keyboardState.PressedKeys.Contains(key);
        }
    }

    public interface IKeyBoardDevice
    {
        void Update();

        bool IsPressed(Key key);
    }
}
