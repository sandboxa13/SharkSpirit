using SharkSpirit.Core;
using SharkSpirit.InputEngine;

namespace SharkSpirit.Engine.Systems.Input
{
    public class InputSystem
    {
        public InputSystem(IContainer container)
        {
            InputManager = new InputManager(container);
        }

        public void UpdateInput()
        {
            InputManager.Update();
        }

        public IInputManager InputManager { get; private set; }
    }
}
