using System;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneItemInspectorViewModel : ViewModelBase
    {
        public SceneItemInspectorViewModel(SceneGraphManager sceneGraphManager)
        {
            sceneGraphManager.SelectedEntityChanged.Subscribe(UpdateSelectedItem);
        }

        private void UpdateSelectedItem(Entity entity)
        {
            SelectedSceneItemViewModel = new SelectedSceneItemViewModel(entity);
        }

        [Reactive] public SelectedSceneItemViewModel SelectedSceneItemViewModel { get; private set; }
    }
}
