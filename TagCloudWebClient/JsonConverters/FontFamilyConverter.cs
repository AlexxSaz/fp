using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TagCloudWebClient.JsonConverters;

internal sealed class FontFamilyJsonConverter : JsonConverter<FontFamily>
{
    public override FontFamily Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start object");
        }

        string? fontFamilyName = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName) continue;
            var propertyName = reader.GetString()!;
            if (propertyName.Equals("FontFamily", StringComparison.OrdinalIgnoreCase) || propertyName.Equals("fontFamily", StringComparison.OrdinalIgnoreCase))
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.String)
                {
                    fontFamilyName = reader.GetString();
                }
                else
                {
                    throw new JsonException("Expected string value for FontFamily");
                }
            }
            else
            {
                reader.Skip();
            }
        }

        if (fontFamilyName == null)
        {
            throw new JsonException("FontFamily property not found");
        }

        return new FontFamily(fontFamilyName);
    }

    public override void Write(Utf8JsonWriter writer, FontFamily value, JsonSerializerOptions options)
    {
        writer.WritePropertyName("FontFamily");
        writer.WriteStringValue(value.Name);
    }
}
