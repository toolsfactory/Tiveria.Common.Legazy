using System;
using System.Xml.Linq;
using Tiveria.Common.Extensions;

namespace Tiveria.Common.Configuration
{
    public class XmlConfigurationSection : IConfigurationSection
    {
        #region private Members
        private XElement _XElement;
        private XmlConfigurationSection _ParentSection = null;
        private XmlConfiguration _Configuration = null;
        #endregion
        #region internal initializers
        private XmlConfigurationSection(XElement xElement)
        {
            _XElement = xElement;
            _XElement.Changed += _XElement_Changed;
        }

        internal XmlConfigurationSection(XElement xElement, XmlConfiguration configuration)
            : this(xElement)
        {
            _Configuration = configuration;
            _XElement = xElement;
            Path = Name;
        }

        internal XmlConfigurationSection(XElement xElement, XmlConfigurationSection parent)
            : this(xElement)
        {
            _Configuration = parent._Configuration;
            _ParentSection = parent;
            Path = String.Format("{0}.{1}", _ParentSection.Path, Name);
        }
        #endregion

        #region public properties
        public string Name { get { return _XElement.Name.ToString(); } }
        public string Path { get; private set; }
        #endregion
        #region public events
        public event EventHandler Changed;
        protected virtual void OnChanged(object sender, EventArgs e)
        {
            EventHandler handler = Changed;
            if (handler != null)
                handler(sender, e);
        }
        #endregion
        #region public Methods
        public T GetItem<T>(string name)
        {
            return _XElement.ElementValue<T>(name, default(T));
        }

        public T GetItem<T>(string name, T defaultValue)
        {
            return _XElement.ElementValue<T>(name, defaultValue);
        }

        public IConfigurationSection SetItem<T>(string name, T value)
        {
            var element = _XElement.Element(name);
            if (element == null)
                _XElement.Add(new XElement(name, value));
            else
                element.Value = value.ToString();

            return this;
        }

        public void DeleteItem(string name)
        {
            var element = _XElement.Element(name);
            if (element == null)
                return;

            element.Remove();
        }

        public IConfigurationSection GetSection(string name)
        {
            var element = _XElement.Element(name);
            if (element == null)
            {
                element = new XElement(name);
                _XElement.Add(element);
            }
            return new XmlConfigurationSection(element, this);
        }

        public void DeleteSection(string name)
        {
            var element = _XElement.Element(name);
            if (element == null)
                return;

            element.Remove();
        }
        #endregion

        void _XElement_Changed(object sender, XObjectChangeEventArgs e)
        {
            OnChanged(this, new EventArgs());
        }


    }
}
