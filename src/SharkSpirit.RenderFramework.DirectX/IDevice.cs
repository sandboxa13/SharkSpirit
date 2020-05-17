using System;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharkSpirit.RenderFramework.DirectX
{
    public interface IDevice
    {
        Device GetDevice();
        DeviceContext GetDeviceContext();
        uint GetTextureId();
        void Initialize();
        void Clear();
        Matrix GetProjection();
        void Flush();
        void Clear(TimeSpan timerTotalTime);
        void Reinitialize();
        void DrawSceneInfo(string output);
    }
}