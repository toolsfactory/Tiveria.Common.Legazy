using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;

namespace Tiveria.Common.Extensions
{
    public static class XmlExtensions
    {
        public static XmlNode AppendAttribute(this XmlNode node, string attribute, string value)
        {
            XmlAttribute attrib = node.OwnerDocument.CreateAttribute(attribute);
            attrib.InnerText = value;
            node.Attributes.Append(attrib);
            return node;
        }

        public static XmlNode AppendElement(this XmlNode node, string name, string value)
        {
            XmlNode newnode = node.OwnerDocument.CreateElement(name);
            newnode.InnerText = value;
            node.AppendChild(newnode);
            return node;
        }

        // Sign an XML file. 
        // This document cannot be verified unless the verifying 
        // code has the key with which it was signed.
        public static void AddSignature(this XmlDocument xmlDoc, AsymmetricAlgorithm Key)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (Key == null)
                throw new ArgumentException("Key");

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = Key;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }

        public static Boolean VerifySignature(this XmlDocument xmlDoc, AsymmetricAlgorithm Key)
        {
            // Check arguments.
                if (xmlDoc == null)
                throw new ArgumentException("Doc");
            if (Key == null)
                throw new ArgumentException("Key");

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for
            // the entire XML document.  Throw an exception 
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.  
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature(Key);
        }

        public static T AttributeValue<T>(this XElement element, string attributename, T defaultvalue)
        {
            if (element == null)
                return defaultvalue;

            XAttribute attribute = element.Attribute(attributename);

            if (attribute == null)
                return defaultvalue;

            try
            {
                T value = (T)Convert.ChangeType(attribute.Value, typeof(T));
                return value;
            }
            catch
            { 
                return defaultvalue;
            }
        }

        public static T ElementAttributeValue<T>(this XElement element, string subelementname, string attributename, T defaultvalue)
        {
            if (element == null)
                return defaultvalue;

            element = element.Element(subelementname);

            if (element == null)
                return defaultvalue;

            XAttribute attribute = element.Attribute(attributename);

            if (attribute == null)
                return defaultvalue;

            try
            {
                T value = (T)Convert.ChangeType(attribute.Value, typeof(T));
                return value;
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static T Value<T>(this XElement element, T defaultvalue)
        {
            if (element == null)
                return defaultvalue;

            try
            {
                T value = (T)Convert.ChangeType(element.Value, typeof(T));
                return value;
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static T ElementValue<T>(this XElement element, string subelementname, T defaultvalue)
        {
            if (element == null)
                return defaultvalue;

            element = element.Element(subelementname);

            if (element == null)
                return defaultvalue;

            try
            {
                T value = (T)Convert.ChangeType(element.Value, typeof(T));
                return value;
            }
            catch
            {
                return defaultvalue;
            }
        }

    }
}
