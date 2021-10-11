using System.Text.Json;
using Famtela.Application.Interfaces.Serialization.Options;

namespace Famtela.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}