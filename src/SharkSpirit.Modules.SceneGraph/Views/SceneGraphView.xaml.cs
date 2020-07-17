using System.Windows.Controls;
using SharkSpirit.Modules.Core.AvalonDock;

namespace SharkSpirit.Modules.SceneGraph.Views
{
    /// <summary>
    /// Interaction logic for SceneGraphView.xaml
    /// </summary>
    [AvalonDockAnchorable(Strategy = AnchorableStrategy.Right, IsHidden = false, Title = "Scene Graph", Size = 300)]
    public partial class SceneGraphView : UserControl
    {
        public SceneGraphView()
        {
            InitializeComponent();
        }
    }
}
