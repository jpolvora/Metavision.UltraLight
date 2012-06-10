using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UltraLight
{
    internal class InstanceProducer
    {
        private readonly RegistrationInfo _info;
        private readonly UltraLightContainer _container;
        private object _instance;
        private readonly Func<object> _factory;
        private readonly List<Type> _parameters = new List<Type>();
        private bool _done;

        public InstanceProducer(RegistrationInfo info, UltraLightContainer container, object instance, Func<object> factory)
        {
            _info = info;
            _container = container;
            _instance = instance;
            _factory = factory;
        }

        public object GetInstance()
        {
            if (_info.Style == LifeStyle.Transient || _instance == null)
            {
                return CreateNewInstance();
            }
            return _instance;
        }

        object CreateNewInstance()
        {
            if (_info.Style == LifeStyle.External)
                return null;

            if (_factory != null)
            {
                var instancia = _factory();
                if (_info.Style == LifeStyle.Singleton)
                    _instance = instancia;
                return instancia;
            }

            GetConstructorTypeParamaters();

            List<object> args = null;
            if (_parameters.Count > 0)
            {
                args = new List<object>();
                foreach (var type in _parameters)
                {
                    args.Add(_container.Resolve(type));
                }
            }
            var instance = args == null
                               ? Activator.CreateInstance(_info.ConcreteType)
                               : Activator.CreateInstance(_info.ConcreteType, args.ToArray());

            if (_info.Style == LifeStyle.Singleton)
                _instance = instance;

            return instance;
        }

        void GetConstructorTypeParamaters()
        {
            if (_done) return;

            int total = 0;
            ConstructorInfo current = null;
            var constructors = _info.ConcreteType.GetConstructors();
            foreach (var constructorInfo in constructors)
            {
                var count = constructorInfo.GetParameters().Count();
                if (count >= total)
                {
                    total = count;
                    current = constructorInfo;
                }
            }

            if (current == null)
                return;

            foreach (var parameter in current.GetParameters())
            {
                _parameters.Add(parameter.ParameterType);
            }
            _done = true;
        }
    }
}