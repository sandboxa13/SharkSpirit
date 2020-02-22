using DryIoc;

namespace SharkSpirit.Engine
{
    public class Game
    {
        private readonly GameTimer _timer;
        
        public Game(IContainer container)
        {
            _timer = new GameTimer();
            Scene = new Scene(container);
        }
        
        public Scene Scene { get; private set; }

        public void Update()
        {
            _timer.Tick();
            Scene.Draw();
        }
    }
}