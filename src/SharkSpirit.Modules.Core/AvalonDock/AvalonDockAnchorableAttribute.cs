using System;

namespace SharkSpirit.Modules.Core.AvalonDock
{
    public class AvalonDockAnchorableAttribute : Attribute
    {
        public AnchorableStrategy Strategy { get; set; }

        public bool IsHidden { get; set; }

        public string Title { get; set; }

        public double Size { get; set; }
    }
}
