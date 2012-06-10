using System;

namespace UltraLight
{
    public static class UltraLightExtensions
    {
        public static void Register<T>(this UltraLightContainer container, Func<T> functor)
        {
            container.InternalRegister(typeof(T), typeof(T), LifeStyle.Transient, null, () => functor());
        }

        public static void RegisterSingle<T>(this UltraLightContainer container, Func<T> functor)
        {
            container.InternalRegister(typeof(T), typeof(T), LifeStyle.Singleton, null, () => functor());
        }

        public static void RegisterInstance<T>(this UltraLightContainer container, T instance)
        {
            container.InternalRegister(typeof(T), typeof(T), LifeStyle.External, instance, null);
        }

        public static void Register<T>(this UltraLightContainer container)
        {
            container.Register(typeof(T), typeof(T));
        }


        public static void Register<T, T2>(this UltraLightContainer container) where T2 : T
        {
            container.InternalRegister(typeof(T), typeof(T2), LifeStyle.Transient, null, null);
        }

        public static bool Contains<T>(this UltraLightContainer container)
        {
            return container.Contains(typeof(T));
        }
    }
}