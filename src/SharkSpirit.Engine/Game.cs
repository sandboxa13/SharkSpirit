using System;
using SharkSpirit.Core;

namespace SharkSpirit.Engine
{
    public class Game
    {
        private readonly IContainer _container;
        private readonly GameTimer _timer;

        public Game(IContainer container)
        {
            _container = container;
            _timer = new GameTimer();
            
            Scene = new Scene(container);
        }

        public Scene Scene { get; private set; }
        public EventHandler GameUpdated { get; set; }
        
        public void Update()
        {
            _timer.Tick();
            Scene.Draw(_timer);
            
            GameUpdated.Invoke(this, EventArgs.Empty);
        }

        public void Reinitialize(IntPtr resourcePointer)
        {
            _container.RemoveService<WindowHandleContainer>();
            _container.AddService(new WindowHandleContainer(resourcePointer));

            Scene.Reinitialize();
        }

        public IContainer GetContainer()
        {
            return _container;
        }
    }
}