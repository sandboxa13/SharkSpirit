using System;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SelectedSceneItemViewModel : ViewModelBase
    {
        public SelectedSceneItemViewModel(Entity entity)
        {
            Name = entity.Name;
            Id = entity.Id;

            TransformComponentViewModel = new TransformComponentViewModel(entity.TransformComponent);
        }

        [Reactive] public Guid Id { get; private set; }

        [Reactive] public TransformComponentViewModel TransformComponentViewModel { get; private set; }
    }
}