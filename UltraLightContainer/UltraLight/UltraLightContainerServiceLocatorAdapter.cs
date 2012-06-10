using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace UltraLight
{
    public class UltraLightContainerServiceLocatorAdapter : IServiceLocator
    {
        private readonly UltraLightContainer _container;

        public UltraLightContainerServiceLocatorAdapter(UltraLightContainer container)
        {
            _container = container;
        }

        #region Implementation of IServiceProvider

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        #endregion

        #region Implementation of IServiceLocator

        public object GetInstance(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return new[] { _container.Resolve(serviceType) };
        }

        public TService GetInstance<TService>()
        {
            return _container.Resolve<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return _container.Resolve<TService>();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return new[] { _container.Resolve<TService>() };
        }

        #endregion
    }
}