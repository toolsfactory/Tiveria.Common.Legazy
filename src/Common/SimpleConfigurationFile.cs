using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Tiveria.Common
{
    public class SimpleConfigurationFile
    {
        private readonly XElement root;
        private const string _MutexIdentifier = "D017C388-65B9-48E1-A36C-D078BB53CF11";

        private SimpleConfigurationFile()
        {
            this.root = new XElement(ConfigurationName);
        }

        private SimpleConfigurationFile(XElement root)
        {
            this.root = root;
        }

         public XElement this[XName section]
        {
            get
            {
                return root.Element(section) ?? new XElement(section);
            }
        }

         #region Static part
         public static string FileName { get; internal set; }
         public static string ConfigurationName { get; set; }

         private static void CheckInitialized()
        {
            if (String.IsNullOrWhiteSpace(FileName))
                throw new ArgumentException("Filename not set");

            if (String.IsNullOrWhiteSpace(ConfigurationName))
            {
                ConfigurationName = "ApplicationConfiguration";
            }
         }

         private static XDocument LoadWithoutCheckingCharacters(string filename)
         {
             // XDocument.Load(fileName) validates that no invalid characters appear (not even in escaped form),
             // but we need those characters for some obfuscated assemblies.
             using (XmlTextReader r = new XmlTextReader(filename))
             {
                 return XDocument.Load(r);
             }
         }

        /// <summary>
        /// Loads the settings file from a file on the disk.
        /// </summary>
        /// <returns>
        /// An instance used to access the loaded settings.
        /// </returns>
        public static SimpleConfigurationFile Load(string filename, string configurationname="")
        {
            FileName = filename;
            ConfigurationName = configurationname;

            CheckInitialized();

            using (new MutexProtection(_MutexIdentifier + System.IO.Path.GetFileNameWithoutExtension(filename)))
            {
                try
                {
                    XDocument doc = LoadWithoutCheckingCharacters(filename);
                    return new SimpleConfigurationFile(doc.Root);
                }
                catch (IOException)
                {
                    return new SimpleConfigurationFile();
                }
                catch (XmlException)
                {
                    return new SimpleConfigurationFile();
                }
            }
        }

        /// <summary>
        /// Saves a setting section.
        /// </summary>
        public static void SaveSettings(XElement section)
        {
            Update(
                delegate(XElement root)
                {
                    XElement existingElement = root.Element(section.Name);
                    if (existingElement != null)
                        existingElement.ReplaceWith(section);
                    else
                        root.Add(section);
                });
        }

        /// <summary>
        /// Updates the saved settings.
        /// To ensure that no changes done in another instance are overwritten, a reload is performed.
        /// </summary>
        public static void Update(Action<XElement> action)
        {
            CheckInitialized();

            using (new MutexProtection(_MutexIdentifier + System.IO.Path.GetFileNameWithoutExtension(FileName)))
            {
                XDocument doc = CreateDocument();
                action(doc.Root);
                // We can't use XDocument.Save(filename) because that checks for invalid characters, but those can appear
                // in obfuscated assemblies.
                SaveConfiguration(doc);
            }
        }

        private static XDocument CreateDocument()
        {
            XDocument doc;
            try
            {
                doc = LoadWithoutCheckingCharacters(FileName);
            }
            catch (IOException)
            {
                // ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(FileName));
                doc = new XDocument(new XElement(ConfigurationName));
            }
            catch (XmlException)
            {
                doc = new XDocument(new XElement(ConfigurationName));
            }
            doc.Root.SetAttributeValue("application", AssemblyAttributes.Title);
            doc.Root.SetAttributeValue("version", AssemblyAttributes.Version.ToString());
            return doc;
        }

        public static void SaveAs(string filename)
        {
            FileName = filename;
        }

        private static void SaveConfiguration(XDocument doc)
        {
            var xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.IndentChars = "    ";
            xws.NewLineHandling = NewLineHandling.Replace;
            xws.OmitXmlDeclaration = true;
            xws.Encoding = Encoding.UTF8;
            using (XmlWriter writer = XmlWriter.Create(FileName, xws))
            {
                doc.Save(writer);
            }
        }
         #endregion
    }
}