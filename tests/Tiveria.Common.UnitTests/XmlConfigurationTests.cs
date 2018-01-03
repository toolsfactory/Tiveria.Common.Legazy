using System;
using NUnit.Framework;

namespace Tiveria.Common.UnitTests
{
    [TestFixture]
    class XmlConfigurationTests
    {
        [SetUp]
        public void SetUp()
        {
            System.IO.File.Delete("testconfig.xml");
            var p = System.IO.Path.GetFullPath("testconfig.xml");
            Console.Write(p);
        }

        [Test]
        public void CreateConfigurationFile()
        {
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            config.Save();

            Assert.IsTrue(System.IO.File.Exists("testconfig.xml"));
        }

        [Test]
        public void WriteAndReadItem()
        {
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var section = config.RootSection;
            section.SetItem<string>("test", "hello world");
            config.Save();

            var loadconfig = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var loadsection = loadconfig.RootSection;
            Assert.IsTrue(loadsection.GetItem<string>("test") == "hello world");
        }

        [Test]
        public void CreateSection()
        {
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var item0 = config.RootSection
                .SetItem<string>("test", "hello world")
                .GetSection("subitems")
                    .SetItem<string>("subvalue", "value");
            Assert.IsTrue(item0.Path == "Configuration.subitems");
            config.Save();

            var loadconfig = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var item = loadconfig.RootSection
                           .GetSection("subitems")
                               .GetItem<string>("subvalue");
            Assert.IsTrue(item == "value");
        }

        [Test]
        public void DeleteSection()
        {
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            config.RootSection
                .SetItem<string>("test", "hello world")
                .GetSection("subitems")
                    .SetItem<string>("subvalue", "value");
            config.Save();

            var delconfig = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            delconfig.RootSection
                .DeleteSection("subitems");
            delconfig.Save();

            var loadconfig = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var item = loadconfig.RootSection
                           .GetSection("subitems")
                               .GetItem<string>("subvalue", "DEFAULT");
            Assert.IsTrue(item == "DEFAULT");
        }

        [Test]
        public void TreeSplit()
        {
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var section = config.RootSection
                .SetItem<string>("test", "hello world")
                .GetSection("subitems");

            section.SetItem<string>("subvalue", "value");
            var subsection = section.GetSection("test");
            subsection.SetItem<string>("deeptest", "value");

            config.Save();

            var loadconfig = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var item = loadconfig.RootSection
                           .GetSection("subitems")
                               .GetItem<string>("subvalue");
            var item2 = loadconfig.RootSection
                           .GetSection("subitems")
                           .GetSection("test")
                               .GetItem<string>("deeptest");
            Assert.IsTrue(item == "value");
            Assert.IsTrue(item2 == "value");
        }

        [Test]
        public void ParallelChange()
        {
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            var section1 = config.RootSection
                .SetItem<string>("test", "hello world")
                .GetSection("subitems");

            section1.SetItem<string>("subvalue", "value1");

            var section2 = config.RootSection
                .SetItem<string>("test", "hello world")
                .GetSection("subitems");

            section2.SetItem<string>("subvalue", "value2");
            Assert.IsTrue(section1.GetItem<string>("subvalue") == "value2");
        }

        [Test]
        public void Event()
        {
            bool saved = false;
            bool changed = false;
            bool secchanged = false;
            var config = Tiveria.Common.Configuration.XmlConfiguration.FromFile("testconfig.xml");
            config.RootSection
                .SetItem<string>("test", "hello world")
                .GetSection("subitems")
                    .SetItem<string>("subvalue", "value");
            config.Saved += (t, e) => { saved = true; };
            config.Save();
            Assert.IsTrue(saved);

            config.Changed += (t, e) => { changed = true; };
            var sec = config.RootSection
                .GetSection("subitems");
            sec.Changed += (t, e) => { secchanged = true; };
            sec.SetItem<string>("subvalue", "value");
            Assert.IsTrue(changed);
            Assert.IsTrue(secchanged);
        }
    }
}
