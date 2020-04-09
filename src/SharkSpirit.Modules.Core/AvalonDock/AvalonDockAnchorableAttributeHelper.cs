using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkSpirit.Modules.Core.AvalonDock
{
    public static class AvalonDockAnchorableAttributeHelper
    {
        public static bool IsAnchorable(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return GetAvalonDockAnchorableAttribute(obj) != null;
        }

        public static AvalonDockAnchorableAttribute GetAvalonDockAnchorableAttribute(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return  obj.GetType().GetCustomAttributes(typeof(AvalonDockAnchorableAttribute), true).FirstOrDefault()
                as AvalonDockAnchorableAttribute;
        }
    }
}
