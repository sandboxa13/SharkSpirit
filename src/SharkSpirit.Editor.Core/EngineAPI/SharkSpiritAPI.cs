using System.Runtime.InteropServices;

namespace SharkSpirit.Editor.Core.EngineAPI
{
    public static class SharkSpiritAPI
    {
        private const string _dllName = "SharkSpirit.dll";

        [DllImport(_dllName)]
        public extern static void test();
    }
}
