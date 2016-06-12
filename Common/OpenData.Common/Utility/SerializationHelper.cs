using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace OpenData.Utility
{
    /// <summary>
    /// Class with methods for saving and loading objects as 
    /// serialized instances.
    /// </summary>
    public static class SerializationHelper
    {
        public static T DeserializeObjectJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static string SerializeObjectToJson(object o)
        {
            return JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
        }
        /// <summary>
        /// Loads the specified type based on the specified stream.
        /// </summary>
        /// <param name="stream">stream containing the type.</param>
        /// <returns></returns>
        public static T DeserializeObjectFromXml<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Serializes an object to a base64 encoded string.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeToBase64String(object o)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            byte[] serialized = stream.ToArray();
            return Convert.ToBase64String(serialized);
        }

        /// <summary>
        /// Deserializes from base64 string.
        /// </summary>
        /// <param name="base64SerializedObject">The base64 serialized object.</param>
        /// <returns></returns>
        public static T DeserializeFromBase64String<T>(string base64SerializedObject)
        {
            byte[] serialized = Convert.FromBase64String(base64SerializedObject);
            MemoryStream stream = new MemoryStream(serialized);
            stream.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            object o = formatter.Deserialize(stream);
            return (T)o;
        }
        public static string SerializationToSoap(object item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add("", "");

                MemoryStream fs = new MemoryStream();
                XmlWriter writer = XmlWriter.Create(fs, settings);
                XmlSerializer xs = new XmlSerializer(item.GetType());
                xs.Serialize(writer, item, ns);
                return System.Text.Encoding.UTF8.GetString(fs.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
