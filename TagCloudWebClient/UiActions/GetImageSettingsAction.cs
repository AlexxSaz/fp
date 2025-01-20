using System.Net;
using System.Text.Json;
using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloudWebClient.UiActions;

public class GetImageSettingsAction(IImageSettingsProvider imageSettingsProvider) : IApiAction
{
    public string Endpoint => "/settings";

    public string HttpMethod => "GET";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var settingsResult = imageSettingsProvider.GetImageSettings();
        if (settingsResult.IsSuccess)
        {
            settingsResult.Then(settings => JsonSerializer.Serialize(outputStream, settings));
            return (int)HttpStatusCode.OK;
        }

        throw new Exception(settingsResult.Error);
    }
}