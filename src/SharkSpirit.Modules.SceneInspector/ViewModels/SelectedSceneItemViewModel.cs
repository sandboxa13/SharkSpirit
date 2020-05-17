using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
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

            TransformComponentViewModel = new TransformComponentViewModel(entity.TransformComponent);

            this.WhenAnyValue(model => model.SelectedColor)
                .Skip(1)
                .Subscribe(color =>
                {
                    entity.MaterialComponent.Color = new Vector4(color.ScR, color.ScG, color.ScB, color.ScA);
                });
        }

        [Reactive] public Guid Id { get; private set; }

        [Reactive] public TransformComponentViewModel TransformComponentViewModel { get; private set; }

        [Reactive] public Color SelectedColor { get; set; }
    }
}