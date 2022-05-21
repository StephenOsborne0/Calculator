namespace Shared.Models
{
    public class SerializableKeyValuePair : ISerializableKeyValuePair
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public SerializableKeyValuePair(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
