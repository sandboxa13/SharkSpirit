using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Engine.Components;
using SharkSpirit.Engine.Systems.Scripts;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.ViewModels.Components;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SelectedSceneItemViewModel : ViewModelBase
    {
        public SelectedSceneItemViewModel(Entity entity)
        {
            Name = entity.Name;
            Id = entity.Id;

            Components = new ObservableCollection<ComponentBaseViewModel>();
            Components.AddRange(
                entity.Components.Select(EntityComponentViewModelsFactory.Create));
            
            this.WhenAnyValue(model => model.SelectedColor)
                .Skip(1)
                .Subscribe(color =>
                {
                    entity.MaterialComponent.Color = new Vector4(color.ScR, color.ScG, color.ScB, color.ScA);
                });
        }
        
        [Reactive] public ObservableCollection<ComponentBaseViewModel> Components { get; set; }

        [Reactive] public Guid Id { get; private set; }

        // todo move to material component
        [Reactive] public Color SelectedColor { get; set; }

        public void Refresh()
        {
            foreach (var componentBaseViewModel in Components)
            {
                componentBaseViewModel?.Refresh();
            }
        }
    }

    public class EntityComponentViewModelsFactory
    {
        public static ComponentBaseViewModel Create(ComponentBase component)
        {
            switch (component.ComponentType)
            {
                case ComponentType.Transform : 
                    return new TransformComponentViewModel(component as TransformComponent);
                case ComponentType.Script : 
                    return new ScriptComponentViewModel(component as ScriptBase);
                case ComponentType.Material : 
                    return new MaterialComponentViewModel(component as MaterialComponent);
                case ComponentType.Light : 
                    return new LightComponentViewModel(component as LightComponent);
            }

            return null;
        }
    }
}