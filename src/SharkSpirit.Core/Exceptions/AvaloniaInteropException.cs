using System;

namespace SharkSpirit.Core.Exceptions
{
    [Serializable]
    public class AvaloniaInteropException : Exception
    {
        public AvaloniaInteropException()
        {
            
        }
        
        public AvaloniaInteropException(string name)
            : base($"")
        {

        }
        
        public AvaloniaInteropException(string name, string errorType)
            : base($"")
        {

        }
    }
}