using System;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.ViewModels;

namespace SharkSpirit.Modules.SceneInspector.ViewModels
{
    public class SceneGraphEntityViewModel : ViewModelBase
    {
        private readonly Entity _entity;

        public SceneGraphEntityViewModel(Entity entity)
        {
            _entity = entity;
            Name = entity.Name;
            Id = entity.Id;

            var e = System.Windows.Forms.Cursor.Position.X;
        }

        public Guid Id { get; private set; }

        public Entity GetEntity() => _entity;
    }
}