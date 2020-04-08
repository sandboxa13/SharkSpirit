using System.Collections.Generic;
using Xceed.Wpf.AvalonDock.Layout;

namespace SharkSpirit.Modules.Core.AvalonDock
{
    public interface ILayoutAnchorableHelper
    {
        List<LayoutAnchorable> LayoutAnchorables { get; }
        void AddLayoutAnchorable(LayoutAnchorable layoutAnchorable);
    }
}
