using SharkSpirit.Modules.Core.AvalonDock;

namespace SharkSpirit.Modules.SceneInspector.Views
{
    /// <summary>
    /// Interaction logic for SceneInspectorView.xaml
    /// </summary>
    [AvalonDockAnchorable(Strategy = AnchorableStrategy.Right, IsHidden = false, Title = "Item Inspector", Size = 300)]
    public partial class SceneInspectorView 
    {
        public SceneInspectorView()
        {
            InitializeComponent();
        }
    }
}
