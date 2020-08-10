using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Engine.Components;
using SharpDX;

namespace SharkSpirit.Modules.SceneInspector.Logic
{
    public class SceneGraphManager
    {
        private readonly ISubject<Entity> _entityChangedSubject;
        private readonly ISubject<Entity> _entityRemovedSubject;
        private readonly ISubject<Entity> _entityAddedSubject;
        
        public SceneGraphManager(IContainer engineContainer)
        {
            Container = engineContainer;
            _entityChangedSubject = new BehaviorSubject<Entity>(Entity.Empty(Container));
            _entityRemovedSubject = new BehaviorSubject<Entity>(Entity.Empty(Container));
            _entityAddedSubject = new BehaviorSubject<Entity>(Entity.Empty(Container));
        }

        public Entity SelectedEntity { get; private set; }
        public IContainer Container { get; private set; }
        public IObservable<Entity> SelectedEntityChanged => _entityChangedSubject;
        public IObservable<Entity> EntityRemovedObservable => _entityRemovedSubject;
        public IObservable<Entity> EntityAddedObservable => _entityAddedSubject;
        public IEnumerable<Entity> GetSceneEntities() => Container.GetService<IScene>().Entities;
        public IEnumerable<CameraComponent> GetSceneCameras() => Container.GetService<IScene>().Cameras;

        public void ChangeSelectedItem(Entity entity)
        {
            SelectedEntity = entity;

            _entityChangedSubject.OnNext(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entityRemovedSubject.OnNext(entity);
            SelectedEntity = null;

            Container.GetService<IScene>().RemoveEntity(entity);
        }

        public void AddEntity()
        {
            var entity = new Entity(Vector3.Zero, Container, $"Cube № {GetSceneEntities().Count() + 1}");

            Container.GetService<IScene>().AddEntityAsync(entity);

            _entityAddedSubject.OnNext(entity);
        }

        public void SelectCamera(Entity entity)
        {
            Container.GetService<IScene>().SelectCamera(entity);

            _entityChangedSubject.OnNext(entity);
        }

        public void AddCamera()
        {
            Container.GetService<IScene>().AddCamera();
        }
    }
}
