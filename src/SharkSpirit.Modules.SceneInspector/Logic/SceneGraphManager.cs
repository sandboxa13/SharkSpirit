using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using SharkSpirit.Core;
using SharkSpirit.Engine;

namespace SharkSpirit.Modules.SceneInspector.Logic
{
    public class SceneGraphManager
    {
        private readonly IContainer _engineContainer;
        private readonly ISubject<Entity> _entityChangedSubject;
        public SceneGraphManager(IContainer engineContainer)
        {
            _engineContainer = engineContainer;
            _entityChangedSubject = new BehaviorSubject<Entity>(Entity.Empty());
        }

        public Entity SelectedEntity { get; private set; }

        public IObservable<Entity> SelectedEntityChanged => _entityChangedSubject;

        public IEnumerable<Entity> GetSceneEntities()
        {
            return _engineContainer.GetService<IScene>().Entities;
        }

        public void ChangeSelectedItem(Entity entity)
        {
            SelectedEntity = entity;

            _entityChangedSubject.OnNext(entity);
        }
    }
}
