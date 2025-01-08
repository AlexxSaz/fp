using System.Net;
using System.Text.Json;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloudWebClient.UiActions;

public class GetImageSettingsAction(IImageSettingsProvider imageSettingsProvider) : IApiAction
{
    public string Endpoint => "/settings";
        
    public string HttpMethod => "GET";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var settings = imageSettingsProvider.GetImageSettings();
        JsonSerializer.Serialize(outputStream, settings);
        return (int)HttpStatusCode.OK;
    }
}