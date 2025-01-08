using System.Net;
using System.Text.Json;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudWebClient.JsonConverters;

namespace TagCloudWebClient.UiActions;

public class UpdateImageSettingsAction(IImageSettingsProvider imageSettingsProvider) : IApiAction
{
    private readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { Converters = { new FontFamilyJsonConverter() } };

    public string Endpoint => "/settings";
    public string HttpMethod => "PUT";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var updatedSettings = JsonSerializer.Deserialize<ImageSettings>(inputStream, _jsonSerializerOptions);
        if (updatedSettings != null) imageSettingsProvider.SetImageSettings(updatedSettings);
        var settings = imageSettingsProvider.GetImageSettings();
        JsonSerializer.Serialize(outputStream, settings);

        return (int)HttpStatusCode.OK;
    }
}