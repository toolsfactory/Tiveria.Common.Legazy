using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tiveria.Common.Bootstrapper;

namespace Tiveria.Common.Bootstrapper.UnitTests
{
    [TestFixture]
    public class BootstrapperTests
    {
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
            Booty.Shutdown();
        }

        [TearDown]
        public void TearDown()
        {
        }
        #endregion

        #region Tests
        [Test]
        public void SimpleCreate()
        {
            Assert.IsNotNull(Booty.Create()) ;
        }

        [Test]
        public void CreatIsCorrectInstance()
        {
            Assert.IsInstanceOf<IBootstrapperConfiguration>(Booty.Create());
        }

        [Test]
        public void CreateAndCheckInitialization()
        {
            Booty.Create();
            Assert.IsNull(Booty.Container);
            Assert.AreEqual(false, Booty.Started);
            Assert.AreEqual(false, Booty.Stopped);
        }
        #endregion

    }
}
