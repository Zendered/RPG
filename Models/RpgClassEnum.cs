using System.Text.Json.Serialization;

namespace RPG.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClassEnum
    {
        Knight,
        Mage,
        Cleric
    }
}
