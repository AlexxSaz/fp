using System.Net;
using System.Text.Json;
using ResultTools;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudWebClient.JsonConverters;

namespace TagCloudWebClient.UiActions;

public class UpdateImageSettingsAction(IImageSettingsProvider imageSettingsProvider) : IApiAction
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { Converters = { new FontFamilyJsonConverter() } };

    public string Endpoint => "/settings";
    public string HttpMethod => "PUT";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var updatedSettings = JsonSerializer.Deserialize<ImageSettings>(inputStream, jsonSerializerOptions);
        if (updatedSettings != null) imageSettingsProvider.SetImageSettings(updatedSettings);
        var settingsResult = imageSettingsProvider.GetImageSettings();
        if (settingsResult.IsSuccess)
        {
            settingsResult.Then(settings => JsonSerializer.Serialize(outputStream, settings));
            return (int)HttpStatusCode.OK;
        }

        throw new Exception(settingsResult.Error);
    }
}