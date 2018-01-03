using System.IO;

namespace Tiveria.Common
{
    public static class SerializationHelpers
    {
        public static T CreateFromFile<T>(string aFileName)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer mySerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (FileStream myFileStream = new FileStream(aFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    T result = (T)mySerializer.Deserialize(myFileStream);
                    myFileStream.Close();
                    return result;
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static T CreateFromString<T>(string text)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer mySerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (var stream = new StringReader(text))
                {
                    T result = (T)mySerializer.Deserialize(stream);
                    stream.Close();
                    return result;
                }
            }
            catch
            {
                return default(T);
            }
        }
        public static bool SaveToFile<T>(T value, string aFileName)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer mySerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (StreamWriter myWriter = new StreamWriter(aFileName))
                {
                    mySerializer.Serialize(myWriter, value);
                    myWriter.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
