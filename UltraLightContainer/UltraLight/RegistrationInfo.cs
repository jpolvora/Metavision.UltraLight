using System;

namespace UltraLight
{
    internal class RegistrationInfo
    {
        private readonly UltraLightContainer _container;
        private readonly Type _concrete;
        private readonly LifeStyle _style;

        private readonly InstanceProducer _producer;

        public RegistrationInfo(Type concrete,
                                LifeStyle style,
                                UltraLightContainer container,
                                object instance = null,
                                Func<object> factory = null)
        {

            _concrete = concrete;
            _style = style;
            _container = container;

            if (instance != null)
                _style = LifeStyle.External;

            _producer = new InstanceProducer(this, _container, instance, factory);
        }

        public Type ConcreteType
        {
            get { return _concrete; }
        }

        public LifeStyle Style
        {
            get { return _style; }
        }

        public object GetInstance()
        {
            return _producer.GetInstance();
        }
    }
}