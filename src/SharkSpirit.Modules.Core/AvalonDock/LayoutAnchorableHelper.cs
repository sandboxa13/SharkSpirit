using System.Collections.Generic;
using Xceed.Wpf.AvalonDock.Layout;

namespace SharkSpirit.Modules.Core.AvalonDock
{
    public class LayoutAnchorableHelper : ILayoutAnchorableHelper
    {
        public List<LayoutAnchorable> LayoutAnchorables { get; }

        public LayoutAnchorableHelper()
        {
            LayoutAnchorables = new List<LayoutAnchorable>();
        }

        public void AddLayoutAnchorable(LayoutAnchorable layoutAnchorable) => LayoutAnchorables.Add(layoutAnchorable);
    }
}
