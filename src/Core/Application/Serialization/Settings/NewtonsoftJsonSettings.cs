
using Famtela.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace Famtela.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}