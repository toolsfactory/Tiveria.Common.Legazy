using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Tiveria.Common.Extensions;

namespace Tiveria.Common.Update
{
    [Serializable]
    public class UpdatePackageInfo
    {
        public string Title
        { get; set; }

        public string Description
        {get;set;}

        public string Version
        {get;set;}

        public string ReleaseNotesUri
        {get;set;}

        public DateTime PublishDate
        { get; set; }

        public string UpdateFileName
        {get; set;}

        public string UpdateFileUri
        {get;set;}

        public string UpdateFileSignature
        {get;set;}

        public string ToXml()
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(UpdatePackageInfo));
                StringWriter myWriter = new StringWriter();
                mySerializer.Serialize(myWriter, this);
                return myWriter.ToString();
            }
            catch
            {
                return "";
            }
        }

        public bool ToFile(string filename)
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(UpdatePackageInfo));
                StreamWriter myWriter = new StreamWriter(filename);
                mySerializer.Serialize(myWriter, this);
                myWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Convert an existing Updatedescription to xml and sign it
        public string ToXml(string rsakey)
        {
            if (String.IsNullOrWhiteSpace(rsakey))
                throw new ArgumentException("Empty rsakey provided");

            RSACryptoServiceProvider rsa;
            try
            {
                rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(rsakey);
            }
            catch
            {
                throw new ArgumentException("Invalid rsakey provided");
            }

            if (rsa.PublicOnly)
                throw new ArgumentException("rsakey has only public part");

            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(this.ToXml());
                xdoc.AddSignature(rsa);
                return xdoc.OuterXml;
            }
            catch
            { return ""; }
        }

        public bool ToFile(string filename, string rsakey)
        {
            string xmldata = ToXml(rsakey);

            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                    sw.WriteLine(xmldata);
                return true;
            }
            catch
            { return false; }
        }

        public override string ToString()
        {
            return String.Format("(UpdatePackageInfo) Title: {0}, Version:{1}, UpdateFileName: {2}", Title, Version, UpdateFileName);
        }
        #region Static Methods

        public static UpdatePackageInfo FromXml(string xmldata)
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(UpdatePackageInfo));
                StringReader reader = new StringReader(xmldata);
                UpdatePackageInfo result = (UpdatePackageInfo)mySerializer.Deserialize(reader);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static UpdatePackageInfo FromXml(string xmldata, string rsakey)
        {
            if (String.IsNullOrWhiteSpace(xmldata))
                throw new ArgumentException("Empty xmldata provided");

            if (String.IsNullOrWhiteSpace(rsakey))
                throw new ArgumentException("Empty rsakey provided");

            RSACryptoServiceProvider rsa;
            try
            {
                rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(rsakey);
            }
            catch
            {
                throw new ArgumentException("Invalid rsakey provided");
            }

            XmlDocument xdoc;
            try
            {
                xdoc = new XmlDocument();
                xdoc.LoadXml(xmldata);
                if (!xdoc.VerifySignature(rsa))
                    return null;

                return UpdatePackageInfo.FromXml(xdoc.OuterXml);
            }
            catch
            {
                return null;
            }
        }
        public static UpdatePackageInfo FromFile(string filename)
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(UpdatePackageInfo));
                FileStream myFileStream = new FileStream(filename, FileMode.Open);
                UpdatePackageInfo result = (UpdatePackageInfo)mySerializer.Deserialize(myFileStream);
                myFileStream.Close();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static UpdatePackageInfo FromFile(string filename, string rsakey)
        {
            if (!File.Exists(filename))
                throw new ArgumentException("file doesnt exist");

            string xmldata;
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                    xmldata = sr.ReadToEnd();
            }
            catch
            {
                return null;
            }

            return FromXml(xmldata, rsakey);
        }
        #endregion
    }
}
