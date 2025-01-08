using System.Text.Json;
using System.Text.Json.Serialization;
using TagCloud.Infrastructure.Tags;

namespace TagCloudWebClient.JsonConverters;

public class WordTagJsonConverter : JsonConverter<StandardWordTag>
{
    public override StandardWordTag? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, StandardWordTag value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value));
    }
}