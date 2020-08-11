using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine.Components;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace SharkSpirit.Modules.SceneInspector.ViewModels.Components
{
    public class LightComponentViewModel : ComponentBaseViewModel
    {
        private readonly LightComponent _lightComponent;
        
        public LightComponentViewModel(LightComponent lightComponent) : base(lightComponent)
        {
            _lightComponent = lightComponent;
            
            DiffuseIntensity = lightComponent.DiffuseIntensity * 100;
            
            DiffuseColor = Color.FromArgb( (byte) lightComponent.DiffuseColor.X, (byte) lightComponent.DiffuseColor.Y, (byte) lightComponent.DiffuseColor.Z, 1);

            AmbientX = Math.Round(lightComponent.Ambient.X, 3);
            AmbientY = Math.Round(lightComponent.Ambient.Y, 3) ;
            AmbientZ = Math.Round(lightComponent.Ambient.Z, 3) ;
            
            AttConst = lightComponent.AttConst * 100;
            AttLin = lightComponent.AttLin * 100;
            AttQuad = lightComponent.AttQuad * 100;
            
            this.WhenAnyValue(model => model.DiffuseIntensity)
                .Subscribe(value => { lightComponent.DiffuseIntensity = value / 100; });
            
            this.WhenAnyValue(model => model.AttConst)
                .Subscribe(value => { lightComponent.AttConst = value/ 100; });
            this.WhenAnyValue(model => model.AttLin)
                .Subscribe(value => { lightComponent.AttLin = value/ 100; });
            this.WhenAnyValue(model => model.AttQuad)
                .Subscribe(value => { lightComponent.AttQuad = value/ 100; });
            
            this.WhenAnyValue(model => model.DiffuseColor).Subscribe(color =>
            {
                lightComponent.DiffuseColor = new Vector3(color.R, color.G, color.B);
            });
            
            this.WhenAnyValue(model => model.AmbientX)
                .Subscribe(value => { lightComponent.Ambient = new Vector3((float) value, lightComponent.Ambient.Y, lightComponent.Ambient.Z); });
            this.WhenAnyValue(model => model.AmbientY)
                .Subscribe(value => { lightComponent.Ambient = new Vector3(lightComponent.Ambient.X, (float) value , lightComponent.Ambient.Z); });
            this.WhenAnyValue(model => model.AmbientZ)
                .Subscribe(value => { lightComponent.Ambient = new Vector3(lightComponent.Ambient.X, lightComponent.Ambient.Y, (float) value ); });
        }
        
        public override void Refresh()
        {
           
        }
        
        [Reactive] public float DiffuseIntensity { get; set; }
        
        [Reactive] public double AmbientX { get; set; }
        [Reactive] public double AmbientY { get; set; }
        [Reactive] public double AmbientZ { get; set; }
        
        [Reactive] public float AttConst { get; set; }
        [Reactive] public float AttLin { get; set; }
        [Reactive] public float AttQuad { get; set; }
        
        [Reactive] public Color DiffuseColor { get; set; }
    }
}