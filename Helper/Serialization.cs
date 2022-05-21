using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Helper
{
    public class Serialization
    {
        public static void SerializeToFile(object obj, string filepath)
        {
            var ext = Path.GetExtension(filepath);
            switch (ext)
            {
                case ".json":
                {
                    SerializeObjectToJsonFile(obj, filepath);
                    break;
                }
                case ".xml":
                default:
                {
                    SerializeObjectToXmlFile(obj, filepath);
                    break;
                }
            }
        }

        public static T DeserializeFile<T>(string filepath) where T : class
        {
            var ext = Path.GetExtension(filepath);
            switch (ext)
            {
                case ".json":
                    return DeserializeJsonFile<T>(filepath);
                case ".xml":
                default:
                    return DeserializeXmlFile<T>(filepath);
            }
        }

        public static void SerializeObjectToXmlFile(object obj, string filepath, bool noNamespace = true)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var settings = new XmlWriterSettings {Indent = true, OmitXmlDeclaration = true};
            var xmlWriter = XmlWriter.Create(filepath, settings);

            var xmlSerializer = new XmlSerializer(obj.GetType());
			
            if (noNamespace)
                xmlSerializer.Serialize(xmlWriter, obj, ns);
            else
                xmlSerializer.Serialize(xmlWriter, obj);
        }

        public static void SerializeObjectToJsonFile(object obj, string filepath)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }

        public static T DeserializeXmlFile<T>(string filepath) where T : class =>
            (T) new XmlSerializer(typeof(T)).Deserialize(new StreamReader(filepath));

        public static T DeserializeJsonFile<T>(string filepath) where T : class =>
            JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath));

        public static string SerializeObjectToJsonString(object obj) => 
            JsonConvert.SerializeObject(obj, Formatting.Indented);

        public static T DeserializeJsonString<T>(string jsonString) where T : class =>
            JsonConvert.DeserializeObject<T>(jsonString);
    }
}
