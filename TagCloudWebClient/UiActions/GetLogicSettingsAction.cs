using System.Net;
using System.Text.Json;
using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudWebClient.JsonConverters;

namespace TagCloudWebClient.UiActions;

public class GetLogicSettingsAction(ILogicSettingsProvider logicSettingsProvider) : IApiAction
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { Converters = { new PointGeneratorTypeConverter() } };

    public string Endpoint => "/logic";

    public string HttpMethod => "GET";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var settingsResult = logicSettingsProvider.GetLogicSettings();
        if (settingsResult.IsSuccess)
        {
            settingsResult.Then(settings =>
            {
                JsonSerializer.Serialize(outputStream, settings, jsonSerializerOptions);
            });
            return (int)HttpStatusCode.OK;
        }

        throw new Exception(settingsResult.Error);
    }
}