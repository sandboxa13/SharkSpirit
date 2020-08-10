using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine.Components;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace SharkSpirit.Modules.SceneInspector.ViewModels.Components
{
    public class MaterialComponentViewModel : ComponentBaseViewModel
    {
        public MaterialComponentViewModel(MaterialComponent materialComponent) : base(materialComponent)
        {
            SelectedColor = Color.FromArgb((byte) materialComponent.Color.W, (byte) materialComponent.Color.X, (byte) materialComponent.Color.Y, (byte) materialComponent.Color.Z);
            SpecularIntensity = materialComponent.SpecularIntensity * 10;
            SpecularPower = materialComponent.SpecularPower;
            
            this.WhenAnyValue(model => model.SelectedColor).Subscribe(color =>
            {
                materialComponent.Color = new Vector4(color.R, color.G, color.B, color.A);
            });
            this.WhenAnyValue(model => model.SpecularIntensity).Subscribe(specularIntens =>
            {
                materialComponent.SpecularIntensity = specularIntens / 10;
            });
            this.WhenAnyValue(model => model.SpecularPower).Subscribe(specPower =>
            {
                materialComponent.SpecularPower = specPower ;
            });
        }
        
        [Reactive] public Color SelectedColor { get; set; }
        [Reactive] public float SpecularIntensity { get; set; }
        [Reactive] public float SpecularPower { get; set; }
    }
}