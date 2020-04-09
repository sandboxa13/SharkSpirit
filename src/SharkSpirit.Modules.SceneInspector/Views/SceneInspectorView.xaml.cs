using System.Windows.Controls;
using SharkSpirit.Modules.Core.AvalonDock;

namespace SharkSpirit.Modules.SceneInspector.Views
{
    /// <summary>
    /// Interaction logic for SceneInspectorView.xaml
    /// </summary>
    [AvalonDockAnchorable(Strategy = AnchorableStrategy.Right, IsHidden = false, Title = "Scene Inspector", Size = 300)]
    public partial class SceneInspectorView : UserControl
    {
        public SceneInspectorView()
        {
            InitializeComponent();
        }
    }
}
