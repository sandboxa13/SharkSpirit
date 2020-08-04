using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Engine.Components;
using SharpDX;

namespace SharkSpirit.Modules.SceneInspector.Logic
{
    public class SceneGraphManager
    {
        private readonly IContainer _engineContainer;
        private readonly ISubject<Entity> _entityChangedSubject;
        private readonly ISubject<Entity> _entityRemovedSubject;
        private readonly ISubject<Entity> _entityAddedSubject;
        public SceneGraphManager(IContainer engineContainer)
        {
            _engineContainer = engineContainer;
            _entityChangedSubject = new BehaviorSubject<Entity>(Entity.Empty());
            _entityRemovedSubject = new BehaviorSubject<Entity>(Entity.Empty());
            _entityAddedSubject = new BehaviorSubject<Entity>(Entity.Empty());
        }

        public Entity SelectedEntity { get; private set; }

        public IObservable<Entity> SelectedEntityChanged => _entityChangedSubject;
        public IObservable<Entity> EntityRemovedObservable => _entityRemovedSubject;
        public IObservable<Entity> EntityAddedObservable => _entityAddedSubject;
        public IEnumerable<Entity> GetSceneEntities() => _engineContainer.GetService<IScene>().Entities;
        public IEnumerable<CameraComponent> GetSceneCameras() => _engineContainer.GetService<IScene>().Cameras;

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

        public void AddEntity()
        {
            var entity = new Entity(Vector3.Zero, _engineContainer, $"Cube № {GetSceneEntities().Count() + 1}");

            _engineContainer.GetService<IScene>().AddEntityAsync(entity);

            _entityAddedSubject.OnNext(entity);
        }

        public void SelectCamera(Entity entity)
        {
            _engineContainer.GetService<IScene>().SelectCamera(entity);

            _entityChangedSubject.OnNext(entity);
        }

        public void AddCamera()
        {
            _engineContainer.GetService<IScene>().AddCamera();
        }
    }
}
