using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;

namespace Tiveria.Common.Bootstrapper.UnitTests
{
    [TestFixture]
    public class PluginStoreTests
    {
        private IBootstrapperPlugin _Plugin1;
        private IBootstrapperPlugin _Plugin2;
        private IBootstrapperPlugin _Plugin3;
        private IBootstrapperContainerPlugin _ContainerPlugin;

        private BootstrapperPluginsStore _Store;

        #region One time Setup & Teardown
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _Plugin1 = Substitute.For<IBootstrapperPlugin>();
            _Plugin2 = Substitute.For<IBootstrapperPlugin>();
            _Plugin3 = Substitute.For<IBootstrapperPlugin>();
            _ContainerPlugin = Substitute.For<IBootstrapperContainerPlugin>();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        { }
        #endregion

        #region Each time Setup & Teardown
        [SetUp]
        public void Setup()
        {
            _Store = new BootstrapperPluginsStore();
        }

        [TearDown]
        public void TearDown()
        { }
        #endregion

        #region Add PluginsTests
        [Test]
        public void AddThreePlugins()
        {
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_Plugin3);

            Assert.AreEqual(3, _Store.Count);
        }

        [Test]
        public void AddThreePluginsAndContainer()
        {
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_Plugin3);
            _Store.AddPlugin(_ContainerPlugin);

            Assert.AreEqual(4, _Store.Count);
        }

        [Test]
        public void AddThreePluginsAndNull()
        {
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_Plugin3);
            _Store.AddPlugin(null);

            Assert.AreEqual(3, _Store.Count);
        }
        #endregion

        #region Container Tests
        [Test]
        public void AddAndRetrieveContainer()
        {
            Assert.IsNull(_Store.Container);
            _Store.AddPlugin(_ContainerPlugin);
            Assert.AreEqual(_ContainerPlugin, _Store.Container);
        }
        #endregion

        #region Initialize, Startup & Shutdown calls
        [Test]
        public void CreateThreePluginsAndCheckInitializeOrder()
        {
            var initializeOrder = "";
            _Plugin1.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "1");
            _Plugin2.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "2");
            _Plugin3.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "3");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_Plugin3);

            _Store.InitializePlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("123", initializeOrder);

        }

        [Test]
        public void CreateThreePluginsAndCheckStartupOrder()
        {
            var startupOrder = "";
            _Plugin1.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "1");
            _Plugin2.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "2");
            _Plugin3.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "3");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_Plugin3);

            _Store.StartupPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("123", startupOrder);

        }

        [Test]
        public void CreateThreePluginsAndCheckShutdownOrder()
        {
            var shutdownOrder = "";
            _Plugin1.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "1");
            _Plugin2.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "2");
            _Plugin3.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "3");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_Plugin3);

            _Store.ShutDownPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("321", shutdownOrder);
        }

        #endregion

        #region Initialize, Startup & Shutdown calls with Container
        [Test]
        public void CreateTwoPluginsWithContainerLastAndCheckInitializeOrder()
        {
            var initializeOrder = "";
            _Plugin1.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "1");
            _Plugin2.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "2");
            _ContainerPlugin.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "x");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_ContainerPlugin);

            _Store.InitializePlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("x12", initializeOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerLastAndCheckStartupOrder()
        {
            var startupOrder = "";
            _Plugin1.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "1");
            _Plugin2.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "2");
            _ContainerPlugin.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "x");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_ContainerPlugin);

            _Store.StartupPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("x12", startupOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerLastAndCheckShutdownOrder()
        {
            var shutdownOrder = "";
            _Plugin1.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "1");
            _Plugin2.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "2");
            _ContainerPlugin.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "x");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_ContainerPlugin);

            _Store.ShutDownPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("21x", shutdownOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerFirstAndCheckInitializeOrder()
        {
            var initializeOrder = "";
            _ContainerPlugin.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "x");
            _Plugin1.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "1");
            _Plugin2.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "2");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);
            _Store.AddPlugin(_ContainerPlugin);

            _Store.InitializePlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("x12", initializeOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerFirstAndCheckStartupOrder()
        {
            var startupOrder = "";
            _ContainerPlugin.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "x");
            _Plugin1.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "1");
            _Plugin2.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "2");
            _Store.AddPlugin(_ContainerPlugin);
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);

            _Store.StartupPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("x12", startupOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerFirstAndCheckShutdownOrder()
        {
            var shutdownOrder = "";
            _ContainerPlugin.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "x");
            _Plugin1.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "1");
            _Plugin2.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "2");
            _Store.AddPlugin(_ContainerPlugin);
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_Plugin2);

            _Store.ShutDownPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("21x", shutdownOrder);
        }
        [Test]
        public void CreateTwoPluginsWithContainerMiddleAndCheckInitializeOrder()
        {
            var initializeOrder = "";
            _ContainerPlugin.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "x");
            _Plugin1.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "1");
            _Plugin2.When(x => x.Initialize(Arg.Any<IBootstrapperContext>())).Do(x => initializeOrder += "2");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_ContainerPlugin);
            _Store.AddPlugin(_Plugin2);

            _Store.InitializePlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("x12", initializeOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerMiddleAndCheckStartupOrder()
        {
            var startupOrder = "";
            _ContainerPlugin.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "x");
            _Plugin1.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "1");
            _Plugin2.When(x => x.Startup(Arg.Any<IBootstrapperContext>())).Do(x => startupOrder += "2");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_ContainerPlugin);
            _Store.AddPlugin(_Plugin2);

            _Store.StartupPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("x12", startupOrder);
        }

        [Test]
        public void CreateTwoPluginsWithContainerMiddleAndCheckShutdownOrder()
        {
            var shutdownOrder = "";
            _ContainerPlugin.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "x");
            _Plugin1.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "1");
            _Plugin2.When(x => x.Shutdown(Arg.Any<IBootstrapperContext>())).Do(x => shutdownOrder += "2");
            _Store.AddPlugin(_Plugin1);
            _Store.AddPlugin(_ContainerPlugin);
            _Store.AddPlugin(_Plugin2);

            _Store.ShutDownPlugins(Substitute.For<IBootstrapperContext>());
            Assert.AreEqual("21x", shutdownOrder);
        }        
        #endregion
    }
}
