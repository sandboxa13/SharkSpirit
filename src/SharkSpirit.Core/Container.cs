using System;
using System.Collections.Generic;

namespace SharkSpirit.Core
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, object> registeredService = new Dictionary<Type, object>();

        /// <inheritdoc />
        public T GetService<T>()
            where T : class
        {
            var type = typeof(T);
            lock (registeredService)
            {
                if (registeredService.ContainsKey(type))
                    return (T)registeredService[type];
            }

            return null;
        }

        /// <inheritdoc />
        public void AddService<T>(T service)
            where T : class
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var type = typeof(T);
            lock (registeredService)
            {
                if (registeredService.ContainsKey(type))
                    throw new ArgumentException("Container is already contains type", nameof(type));
                registeredService.Add(type, service);
            }
        }

        /// <inheritdoc />
        public void RemoveService<T>()
            where T : class
        {
            var type = typeof(T);
            object oldService;
            lock (registeredService)
            {
                if (registeredService.TryGetValue(type, out oldService))
                    registeredService.Remove(type);
            }
        }
    }

    public interface IContainer
    {
        void AddService<T>(T service) where T : class;
        T GetService<T>() where T : class;
        void RemoveService<T>() where T : class;
    }
}