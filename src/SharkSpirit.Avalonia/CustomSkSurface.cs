using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.OpenGL;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SharkSpirit.Core;
using SkiaSharp;

namespace SharkSpirit.Avalonia
{
    public class CustomSkSurface : Control
    {
        private static MainWindowViewModel _mainWindowViewModel;

        public CustomSkSurface()
        {
             _mainWindowViewModel = DataContext as MainWindowViewModel;
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            
            _mainWindowViewModel = DataContext as MainWindowViewModel;
        }

        public override void Render(DrawingContext context)
        {
            context.Custom(new CustomDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height ), _mainWindowViewModel.GetSystemConfiguration()));
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }
        
        private class CustomDrawOp : ICustomDrawOperation
        {
            private readonly IConfiguration _systemConfiguration;

            public CustomDrawOp(Rect bounds, IConfiguration systemConfiguration)
            {
                _systemConfiguration = systemConfiguration;
                Bounds = bounds;
            }

            public void Dispose()
            {
                // No-op
            }

            public Rect Bounds { get; }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => false;

            public void Render(IDrawingContextImpl context)
            {
                _mainWindowViewModel.Update();

                var textureId = _mainWindowViewModel.GetTextureId();
                
                var grContext = ((ISkiaDrawingContextImpl) context).GrContext;

                using var texture = new GRBackendTexture(
                    (int)_systemConfiguration.Width, 
                    (int)_systemConfiguration.Height,
                    false,
                    new GRGlTextureInfo(GlConsts.GL_TEXTURE_2D, textureId, GlConsts.GL_RGBA8));
                
                using var surface = SKSurface.Create(grContext, texture, SKColorType.Rgba8888);
                var canvas = ((ISkiaDrawingContextImpl) context)?.SkCanvas;
                
                //canvas?.Clear(new SKColor(0, 0, 0, 1));
                canvas?.DrawSurface(surface, 0, 0, new SKPaint());
                canvas?.Flush();
            }
        }
    }
}