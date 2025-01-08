using System.Text.Json;
using System.Text.Json.Serialization;
using TagCloud.Logic.PointGenerators;

namespace TagCloudWebClient.JsonConverters;

public class PointGeneratorTypeConverter : JsonConverter<PointGeneratorType>
{
    public override PointGeneratorType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException($"Expected number, got {reader.TokenType}");
        }

        var value = reader.GetInt32();

        return value switch
        {
            1 => PointGeneratorType.Spiral,
            2 => PointGeneratorType.Astroid,
            _ => throw new JsonException($"Unknown PointGeneratorType value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, PointGeneratorType value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((int)value + 1);
    }
}