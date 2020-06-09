using System.Drawing;
using System.Windows.Forms;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharpDX.Windows;
using Configuration = SharkSpirit.Core.Configuration;

namespace SharkSpirit.TestConsole
{
    class Program
    {

        static void Main()
        {
            var testApp = new TestApp();

            testApp.Run();
        }
    }

    class TestApp
    {
        private RenderForm _renderForm;
        private Game _game;
        public TestApp()
        {
            Initialize();
        }

        private void Initialize()
        {
            var container = new Container();

            var graphicsConfiguration = new Configuration
            {
                Height = 1080,
                Width = 1920,
                EngineEditorType = EngineEditorType.Default,
                PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders"
            };

            container.AddService(graphicsConfiguration);

            _renderForm = new RenderForm
            {
                StartPosition = FormStartPosition.CenterScreen,
                AutoScaleMode = AutoScaleMode.None,
                Text = "Test",
                Size = new Size((int) graphicsConfiguration.Width, (int) graphicsConfiguration.Height)
            };

            var windowHandle = new WindowHandleContainer(_renderForm.Handle);
            container.AddService(windowHandle);

            _game = new Game(container);
        }

        public void Run()
        {
            _renderForm.Show();

            RenderLoop.Run(_renderForm, () => { _game.Update();});
        }
    }
}
