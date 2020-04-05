using System;
using System.Runtime.InteropServices;

namespace SharkSpirit.Core.Extensions
{
    public static class IntPtrExtensions
    {
        public static T GetFunctionByPointer<T>(this IntPtr procAddress) where T : Delegate
        {
            return (T) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
        }
    }
}