using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine.Systems.Scripts;

namespace SharkSpirit.Modules.SceneInspector.ViewModels.Components
{
    public class ScriptComponentViewModel : ComponentBaseViewModel
    {
        public ScriptComponentViewModel(ScriptBase scriptComponent) : base(scriptComponent)
        {
            PathToScript = scriptComponent.PathToScript;

            IsEnabled = true;
            
            this.WhenAnyValue(model => model.IsEnabled)
                .Subscribe(scriptComponent.ChangeIsEnabled);
        }
        
        [Reactive] public string PathToScript { get; set; }
        [Reactive] public bool IsEnabled { get; set; }
    }
}