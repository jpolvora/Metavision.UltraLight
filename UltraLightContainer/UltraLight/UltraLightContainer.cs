using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace UltraLight
{
    public class UltraLightContainer
    {
        readonly Dictionary<Type, RegistrationInfo> _registrations = new Dictionary<Type, RegistrationInfo>();

        #region info for testing

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Contains(Type type)
        {
            return _registrations.ContainsKey(type);
        }

        private RegistrationInfo Get(Type type)
        {
            RegistrationInfo info;
            return _registrations.TryGetValue(type, out info) ? _registrations[type] : null;
        }

        public bool IsSingleton(Type type)
        {
            var info = Get(type);
            return info != null && info.Style == LifeStyle.Singleton;
        }

        public bool IsTransient(Type type)
        {
            var info = Get(type);
            return info != null && info.Style == LifeStyle.Transient;
        }

        public bool IsExternal(Type type)
        {
            var info = Get(type);
            return info != null && info.Style == LifeStyle.External;
        }

        #endregion

        #region Register

        /// <summary>
        /// Register a Type as a Singleton of itself
        /// </summary>
        /// <param name="type"></param>
        public void RegisterSingleton(Type type)
        {
            InternalRegister(type, type, LifeStyle.Singleton, null, null);
        }

        /// <summary>
        /// Singleton Registrations
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="derivedType"></param>
        public void RegisterSingleton(Type baseType, Type derivedType)
        {
            InternalRegister(baseType, derivedType, LifeStyle.Singleton, null, null);
        }

        /// <summary>
        /// Register as Transient
        /// </summary>
        public void Register(Type baseType, Type derivedType)
        {
            InternalRegister(baseType, derivedType, LifeStyle.Transient, null, null);
        }

        internal void InternalRegister(Type baseType, Type derivedType, LifeStyle style, object instance, Func<object> factory)
        {
            var registrationInfo = new RegistrationInfo(derivedType, style, this, instance, () => factory);
            _registrations[baseType] = registrationInfo;
        }

        #endregion

        #region Resolve

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type abstractType)
        {
            if (abstractType.IsGenericType)
            {
                var extractedType = abstractType.GetGenericArguments();
                var newTypeToResolve = abstractType.GetGenericTypeDefinition();
                return ResolveOpenGeneric(abstractType, newTypeToResolve, extractedType[0]);
            }

            if (_registrations.ContainsKey(abstractType))
            {
                var actualReg = _registrations[abstractType];
                return actualReg.GetInstance();
            }
            Register(abstractType, abstractType);
            return Resolve(abstractType);
        }

        private object ResolveOpenGeneric(Type abstractType, Type openGeneric, Type parametro)
        {
            if (_registrations.ContainsKey(openGeneric))
            {
                var reg = _registrations[openGeneric];

                var typeParams = new[] { parametro };
                var constructedType = reg.ConcreteType.MakeGenericType(typeParams);
                var exists = _registrations.ContainsKey(constructedType);
                if (!exists)
                {
                    if (reg.Style == LifeStyle.Singleton)
                        RegisterSingleton(abstractType, constructedType);
                    else Register(abstractType, constructedType);
                }

                return Resolve(constructedType);

            }
            return null;
        }

        #endregion
    }
}