using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;
using SharkSpirit.Modules.SceneInspector.Logic;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneItemInspectorViewModel : ViewModelBase
    {
        private readonly Dictionary<Guid, SelectedSceneItemViewModel> _selectedSceneItemViewModels;
        
        public SceneItemInspectorViewModel(SceneGraphManager sceneGraphManager)
        {
            _selectedSceneItemViewModels = new Dictionary<Guid, SelectedSceneItemViewModel>();
            
            sceneGraphManager.SelectedEntityChanged.Subscribe(UpdateSelectedItem);
        }

        private void UpdateSelectedItem(Entity entity)
        {
            if (_selectedSceneItemViewModels.TryGetValue(entity.Id, out var viewModel))
            {
                SelectedSceneItemViewModel = viewModel;
            }
            else
            {
                SelectedSceneItemViewModel = new SelectedSceneItemViewModel(entity);
                _selectedSceneItemViewModels.Add(entity.Id, SelectedSceneItemViewModel);
            }
        }

        [Reactive] public SelectedSceneItemViewModel SelectedSceneItemViewModel { get; private set; }

        public void Refresh()
        {
            SelectedSceneItemViewModel.Refresh();
        }
    }
}
