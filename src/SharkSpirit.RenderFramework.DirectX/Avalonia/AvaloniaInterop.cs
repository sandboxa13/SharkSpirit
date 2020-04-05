using System;
using Avalonia.OpenGL;
using SharkSpirit.Core;
using SharkSpirit.Core.Exceptions;
using SharkSpirit.Core.Extensions;
using SharkSpirit.Core.Utilities;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.Avalonia
{
    public class AvaloniaInterop : IAvaloniaInterop
    {
        private readonly IContainer _container;
        private readonly IWindowingPlatformGlFeature _platformGlFeature;
        private readonly int _directXVersion;
        private IConfiguration _systemConfiguration;
        private EglContext _eglContext;
        private IntPtr _displayHandle;
        private EglDisplay _eglDisplay;
        private IntPtr _deviceHandle;
        private uint _textureId;

        private delegate IntPtr CreateEglSurfaceDelegate(IntPtr display, int bufferType, IntPtr bufferHandle, IntPtr config, int[] attr);

        private delegate bool GetConfigDelegate(IntPtr display, IntPtr context, int attribute, int[] values);

        private delegate bool GetDeviceDelegate(IntPtr displayHandle, int type, out IntPtr ptr);

        private delegate void GetTexturesDelegate(uint size, out uint texture);

        private delegate void BindTextureDelegate(int type, uint texture);

        private delegate bool BindTexImageDelegate(IntPtr display, IntPtr surface, int type);

        private delegate void GlTexParameteri(int target, int pname, int param);

        
        public AvaloniaInterop(
            IContainer container,
            IWindowingPlatformGlFeature platformGlFeature,
            int directXVersion)
        {
            _container = container;
            _platformGlFeature = platformGlFeature;
            _directXVersion = directXVersion;
        }

        public void Initialize()
        {
            _eglContext = _platformGlFeature.ImmediateContext as EglContext;
            _displayHandle = ((EglDisplay) _eglContext.Display).Handle;
            _eglDisplay = (EglDisplay) _eglContext.Display;
            _systemConfiguration = _container.GetService<IConfiguration>();
            
            // create function for getting EglDevice
            var getEglDeviceFunction = _eglContext.Display.GlInterface
                .GetProcAddress("eglQueryDisplayAttribEXT")
                .GetFunctionByPointer<GetDeviceDelegate>();
            
            // try to get EglDevice
            if (!getEglDeviceFunction(_displayHandle, SharkSpiritEglConsts.EGL_DEVICE_EXT, out var eglDeviceHandle))
            {
                var error = _eglContext.Display.GlInterface.GetError();
                throw new AvaloniaInteropException("Cannot get Egl Device", $"error type - {error}");
            }

            // create function for getting Angle D3Device
            var getAngleD3DeviceFunction = _eglContext.Display.GlInterface
                .GetProcAddress("eglQueryDeviceAttribEXT")
                .GetFunctionByPointer<GetDeviceDelegate>();

            // try to get Angle D3Device
            if (!getAngleD3DeviceFunction(eglDeviceHandle, _directXVersion, out var d3dDeviceHandle))
            {
                var error = _eglContext.Display.GlInterface.GetError();
                throw new AvaloniaInteropException("Cannot get Angle d3device", $"error type - {error}");
            }

            _deviceHandle = d3dDeviceHandle;
        }

        public void InitializeSurface(IntPtr texturePtr)
        {
            var configHandle = GetConfigHandle();

            var eglSurface = CreateEglSurface(configHandle, texturePtr);

            _textureId = BindEglSurfaceToTexture(eglSurface);
        }

        public IntPtr GetDevicePtr() => _deviceHandle;
        public uint GetTextureId() => _textureId;


        #region Helpers

        private IntPtr GetConfigHandle()
        {
            // create function for getting EglContext
            var getEglConfigFunction = _eglContext.Display.GlInterface
                .GetProcAddress("eglQueryContext")
                .GetFunctionByPointer<GetConfigDelegate>();

            // config ID
            var configIDs = new int[1];

            // getting config ID
            if (!getEglConfigFunction(_displayHandle, _eglContext.Context, EglConsts.EGL_CONFIG_ID, configIDs))
            {
                var error = _eglContext.Display.GlInterface.GetError();
                throw new AvaloniaInteropException("Cannot get Egl Config", $"error type - {error}");
            }

            // config attributes
            var attribs = new[]
            {
                EglConsts.EGL_CONFIG_ID, configIDs[0],
                EglConsts.EGL_NONE
            };

            // get config by ID
            _eglDisplay.EglInterface.ChooseConfig(_displayHandle, attribs, out var configHandle, 1, out _);

            return configHandle;
        }
        
        private IntPtr CreateEglSurface(IntPtr configHandle, IntPtr texturePtr)
        {
            // create function for creating egl surface
            var getEglDeviceFunction = _eglContext.Display.GlInterface
                .GetProcAddress("eglCreatePbufferFromClientBuffer")
                .GetFunctionByPointer<CreateEglSurfaceDelegate>();

            // create attributes
            var pBufferAttributes = new[]
            {
                EglConsts.EGL_WIDTH, (int) _systemConfiguration.Width,
                EglConsts.EGL_HEIGHT,(int) _systemConfiguration.Height,
                EglConsts.EGL_TEXTURE_TARGET, EglConsts.EGL_TEXTURE_2D,
                EglConsts.EGL_TEXTURE_FORMAT, EglConsts.EGL_TEXTURE_RGBA,
                EglConsts.EGL_NONE
            };

            // create egl surface            
            return getEglDeviceFunction(_displayHandle, SharkSpiritEglConsts.EGL_D3D_TEXTURE_ANGLE,
                texturePtr, configHandle, pBufferAttributes);
        }
        private uint BindEglSurfaceToTexture(IntPtr eglSurface)
        {
            // create function for getting texture name
            var getTextureNameFunction = _eglContext.Display.GlInterface
                .GetProcAddress("glGenTextures")
                .GetFunctionByPointer<GetTexturesDelegate>();
            getTextureNameFunction(1, out var textures);
            
            
            // create function for getting texture name
            var bindTextureFunction = _eglContext.Display.GlInterface
                .GetProcAddress("glBindTexture")
                .GetFunctionByPointer<BindTextureDelegate>();
            bindTextureFunction(GlConsts.GL_TEXTURE_2D , textures);

            
            // create function for getting texture name
            var bindTexImageFunction = _eglContext.Display.GlInterface
                .GetProcAddress("eglBindTexImage")
                .GetFunctionByPointer<BindTexImageDelegate>();
            if(!bindTexImageFunction(_displayHandle, eglSurface, EglConsts.EGL_BACK_BUFFER))
            {
                                
            }
            
            // create function for getting texture name
            var glTexParameteriFunction = _eglContext.Display.GlInterface
                .GetProcAddress("glTexParameteri")
                .GetFunctionByPointer<GlTexParameteri>();
            glTexParameteriFunction(GlConsts.GL_TEXTURE_2D, GlConsts.GL_TEXTURE_MIN_FILTER, GlConsts.GL_NEAREST);
            glTexParameteriFunction(GlConsts.GL_TEXTURE_2D, GlConsts.GL_TEXTURE_MAG_FILTER, GlConsts.GL_NEAREST);

            return textures;
        }

        #endregion
    }
}