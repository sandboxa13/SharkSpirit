using SharkSpirit.Core;
using SharkSpirit.Engine;

namespace SharkSpirit.Avalonia
{
    public class MainWindowViewModel
    {
        private Game _game;
        private IConfiguration _configuration;

        public MainWindowViewModel(IContainer container)
        {
            _configuration = container.GetService<IConfiguration>();
            
            _game = new Game(container);
        }
        
        public void Update() => _game.Update();
        public uint GetTextureId() => _game.Scene.RenderSystem.Device.GetTextureId();

        public IConfiguration GetSystemConfiguration()
        {
            return _configuration;
        }
    }
}