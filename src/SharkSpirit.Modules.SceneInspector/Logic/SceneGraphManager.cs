using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Windows;
using SharkSpirit.Core;
using SharkSpirit.Engine;

namespace SharkSpirit.Modules.SceneInspector.Logic
{
    public class SceneGraphManager
    {
        private readonly IContainer _engineContainer;
        private readonly ISubject<Entity> _entityChangedSubject;
        private readonly ISubject<Entity> _entityRemovedSubject;
        public SceneGraphManager(IContainer engineContainer)
        {
            _engineContainer = engineContainer;
            _entityChangedSubject = new BehaviorSubject<Entity>(Entity.Empty());
            _entityRemovedSubject = new BehaviorSubject<Entity>(Entity.Empty());
        }

        public Entity SelectedEntity { get; private set; }

        public IObservable<Entity> SelectedEntityChanged => _entityChangedSubject;
        public IObservable<Entity> EntityRemovedObservable => _entityRemovedSubject;
        public IEnumerable<Entity> GetSceneEntities() => _engineContainer.GetService<IScene>().Entities;

        public void ChangeSelectedItem(Entity entity)
        {
            SelectedEntity = entity;

            _entityChangedSubject.OnNext(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entityRemovedSubject.OnNext(entity);
            SelectedEntity = null;

            _engineContainer.GetService<IScene>().RemoveEntity(entity);
        }
    }
}
