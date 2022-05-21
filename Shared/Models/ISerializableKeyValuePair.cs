namespace Shared.Models
{
    public interface ISerializableKeyValuePair
    {
        string Key { get; set; }

        object Value { get; set; }
    }
}
