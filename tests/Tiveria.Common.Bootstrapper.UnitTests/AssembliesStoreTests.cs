using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using System.Reflection;

namespace Tiveria.Common.Bootstrapper.UnitTests
{
    [TestFixture]
    public class AssembliesStoreTests
    {
        private IBootstrapperAssemblyProvider _DummyAssemblyProvider;
        private BootstrapperAssembliesStore _Store;
        #region One time Setup & Teardown
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {

        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        { }
        #endregion

        #region Each time Setup & Teardown
        [SetUp]
        public void Setup()
        {
            _DummyAssemblyProvider = Substitute.For<IBootstrapperAssemblyProvider>();
            _DummyAssemblyProvider.GetAssemblies().Returns(new List<Assembly>());
            _DummyAssemblyProvider.SanitizeAssemblies(Arg.Any<IEnumerable<Assembly>>()).Returns(x => x.Arg<IEnumerable<Assembly>>());

            _Store = new BootstrapperAssembliesStore();
        }

        [TearDown]
        public void TearDown()
        { }
        #endregion

        #region Tests
        [Test]
        public void SetCustomAssemblyProvider()
        {
            _Store.SetAssembliesProvider(_DummyAssemblyProvider);
            _Store.InitializeAssembliesList();

            Assert.AreEqual(0, _Store.Assemblies.Count);
        }

        [Test]
        public void IncludeAssembly()
        {
            _Store.SetAssembliesProvider(_DummyAssemblyProvider);
            _Store.IncludeAssembly(typeof(AssembliesStoreTests).Assembly);
            _Store.InitializeAssembliesList();

            Assert.AreEqual(1, _Store.Assemblies.Count);
            Assert.AreEqual(typeof(AssembliesStoreTests).Assembly.FullName, _Store.Assemblies[0].FullName);
        }

        [Test]
        public void ExcludeAssemblyAfterInclude()
        {
            _Store.SetAssembliesProvider(_DummyAssemblyProvider);
            _Store.IncludeAssembly(typeof(AssembliesStoreTests).Assembly);
            _Store.IncludeAssembly(typeof(NUnit.Framework.Assert).Assembly);
            _Store.IncludeAssembly(typeof(NSubstitute.Substitute).Assembly);
            _Store.ExcludeAssembly("NUnit");

            _Store.InitializeAssembliesList();

            Assert.AreEqual(2, _Store.Assemblies.Count);
            Assert.AreEqual(typeof(AssembliesStoreTests).Assembly.FullName, _Store.Assemblies[0].FullName);
            Assert.AreEqual(typeof(NSubstitute.Substitute).Assembly.FullName, _Store.Assemblies[1].FullName);
        }

        [Test]
        public void ExcludeAssemblyBeforeInclude()
        {
            _Store.SetAssembliesProvider(_DummyAssemblyProvider);
            _Store.ExcludeAssembly("NUnit");
            _Store.IncludeAssembly(typeof(AssembliesStoreTests).Assembly);
            _Store.IncludeAssembly(typeof(NUnit.Framework.Assert).Assembly);
            _Store.IncludeAssembly(typeof(NSubstitute.Substitute).Assembly);

            _Store.InitializeAssembliesList();

            Assert.AreEqual(2, _Store.Assemblies.Count);
            Assert.AreEqual(typeof(AssembliesStoreTests).Assembly.FullName, _Store.Assemblies[0].FullName);
            Assert.AreEqual(typeof(NSubstitute.Substitute).Assembly.FullName, _Store.Assemblies[1].FullName);
        }

        [Test]
        public void AutoFilterMicrosoftSystem()
        {
            _Store.SetAssembliesProvider(_DummyAssemblyProvider);
            _Store.IncludeAssembly(typeof(System.Random).Assembly);
            _Store.IncludeAssembly(typeof(Microsoft.CSharp.CSharpCodeProvider).Assembly);
            _Store.IncludeAssembly(typeof(NSubstitute.Substitute).Assembly);

            _Store.InitializeAssembliesList();

            Assert.AreEqual(1, _Store.Assemblies.Count);
            Assert.AreEqual(typeof(NSubstitute.Substitute).Assembly.FullName, _Store.Assemblies[0].FullName);
        }

        #endregion

    }
}
