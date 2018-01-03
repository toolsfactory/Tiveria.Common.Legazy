using System.Collections;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Tiveria.Common.Collections
{
    public class SerializableHashtable : Hashtable, IXmlSerializable
    {
        public SerializableHashtable()
            : base()
        {

        }
        public SerializableHashtable(int capacity)
            : base(capacity)
        {

        }
        public SerializableHashtable(int capacity, float loadFactor)
            : base(capacity, loadFactor)
        {

        }
        public SerializableHashtable(int capacity, float loadFactor, IHashCodeProvider hcp, IComparer comparer)
            : base(capacity, loadFactor, hcp, comparer)
        {

        }
        public SerializableHashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer)
            : base(capacity, loadFactor, equalityComparer)
        {

        }
        public SerializableHashtable(IHashCodeProvider hcp, IComparer comparer)
            : base(hcp, comparer)
        {

        }
        public SerializableHashtable(IEqualityComparer equalityComparer)
            : base(equalityComparer)
        {

        }
        public SerializableHashtable(int capacity, IHashCodeProvider hcp, IComparer comparer)
            : base(capacity, hcp, comparer)
        {

        }
        public SerializableHashtable(int capacity, IEqualityComparer equalityComparer)
            : base(capacity, equalityComparer)
        {

        }
        public SerializableHashtable(IDictionary d)
            : base(d)
        {

        }
        public SerializableHashtable(IDictionary d, float loadFactor)
            : base(d, loadFactor)
        {

        }
        public SerializableHashtable(IDictionary d, IHashCodeProvider hcp, IComparer comparer)
            : base(d, hcp, comparer)
        {

        }
        public SerializableHashtable(IDictionary d, IEqualityComparer equalityComparer)
            : base(d, equalityComparer)
        {

        }
        public SerializableHashtable(IDictionary d, float loadFactor, IHashCodeProvider hcp, IComparer comparer)
            : base(d, loadFactor, hcp, comparer)
        {

        }
        public SerializableHashtable(IDictionary d, float loadFactor, IEqualityComparer equalityComparer)
            : base(d, loadFactor, equalityComparer)
        {

        }
        protected SerializableHashtable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();
            reader.ReadStartElement("dictionary");

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                string key = reader.ReadElementString("key");
                string value = reader.ReadElementString("value");

                reader.ReadEndElement();
                reader.MoveToContent();

                this.Add(key, value);
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("dictionary");

            foreach (object key in this.Keys)
            {
                object value = this[key];
                writer.WriteStartElement("item");
                writer.WriteElementString("key", key.ToString());
                writer.WriteElementString("value", value.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
