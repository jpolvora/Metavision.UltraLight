using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltraLight.Tests
{
    public class Base
    {

    }

    public class Derived : Base
    {

    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RegisterSingletonOneParameter()
        {
            var container = new UltraLightContainer();
            container.RegisterSingleton(typeof(Base));

            Assert.IsTrue(container.Contains(typeof(Base)));
            Assert.IsTrue(container.IsSingleton(typeof(Base)));
            Assert.IsFalse(container.IsTransient(typeof(Base)));
            Assert.IsFalse(container.IsExternal(typeof(Base)));
        }

        [TestMethod]
        public void RegisterSingletonTwoParams()
        {
            var container = new UltraLightContainer();
            container.Register(typeof(Base), typeof(Derived));

            Assert.IsTrue(container.Contains(typeof(Base)));
            Assert.IsTrue(container.IsTransient(typeof(Base)));
            Assert.IsFalse(container.IsSingleton(typeof(Base)));
            Assert.IsFalse(container.IsExternal(typeof(Base)));
        }


    }
}
