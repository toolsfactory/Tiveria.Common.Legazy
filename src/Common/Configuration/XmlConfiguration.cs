using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Tiveria.Common.Configuration
{
    public class XmlConfiguration : IConfiguration
    {
        #region private members
        private XDocument _Document;
        #endregion

        #region Public Events
        public event System.EventHandler Changed;
        protected virtual void OnChanged(object sender, EventArgs e)
        {
            System.EventHandler handler = Changed;
            if (handler != null)
                handler(sender, e);
        }

        public event System.EventHandler Saved;
        protected virtual void OnSaved(object sender, System.EventArgs e)
        {
            System.EventHandler handler = Saved;
            if (handler != null)
                handler(sender, e);
        }
        #endregion

        public IConfigurationSection RootSection { get; private set; }
        public string ConfigurationName { get; private set; }
        public bool PendingChanges { get; private set; }

        public XmlConfiguration()
        { PendingChanges = false; }

        private void NewConfigurationFile()
        {
            _Document = new XDocument(new XElement("Configuration"));
        }

        public void Save()
        {
            if (String.IsNullOrWhiteSpace(ConfigurationName))
                throw new ArgumentException("ConfigurationName is empty");

            using (XmlWriter writer = XmlWriter.Create(ConfigurationName, GetXws()))
            {
                _Document.Save(writer);
            }

            PendingChanges = false;
            OnSaved(this, new EventArgs());
        }

        public void SaveAs(string configurationName)
        {
            ConfigurationName = configurationName;
            Save();
        }

        private XDocument LoadWithoutCheckingCharacters(string filename)
        {
            // XDocument.Load(fileName) validates that no invalid characters appear (not even in escaped form),
            // but we need those characters for some obfuscated assemblies.
            using (XmlTextReader r = new XmlTextReader(filename))
            {
                return XDocument.Load(r);
            }
        }

        private IConfiguration InternalLoad(string configurationName)
        {
            try
            {
                ConfigurationName = configurationName;
                _Document = LoadWithoutCheckingCharacters(configurationName);
            }
            catch (IOException)
            {
                NewConfigurationFile();
            }
            catch (XmlException)
            {
                NewConfigurationFile();
            }
            RootSection = new XmlConfigurationSection(_Document.Root, this);
            RootSection.Changed += RootSection_Changed;
            return this;
        }

        private void RootSection_Changed(object sender, EventArgs e)
        {
            PendingChanges = true;
            OnChanged(this, new EventArgs());
        }

        private static XmlWriterSettings GetXws()
        {
            var xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.IndentChars = "    ";
            xws.NewLineHandling = NewLineHandling.Replace;
            xws.OmitXmlDeclaration = false;
            xws.Encoding = System.Text.Encoding.UTF8;
            return xws;
        }

        public static IConfiguration FromFile(string filename)
        {
            var configuration = new XmlConfiguration();
            return configuration.InternalLoad(filename);
        }
    }
}