﻿using System.Net;
using System.Text.Json;
using ResultTools;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudWebClient.JsonConverters;

namespace TagCloudWebClient.UiActions;

public class UpdateLogicSettingsAction(ILogicSettingsProvider logicSettingsProvider) : IApiAction
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { Converters = { new PointGeneratorTypeConverter() } };

    public string Endpoint => "/logic";
    public string HttpMethod => "PUT";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var updatedSettings = JsonSerializer.Deserialize<LogicSettings>(inputStream, options: jsonSerializerOptions);
        if (updatedSettings != null) logicSettingsProvider.SetLogicSettings(updatedSettings);
        var settingsResult = logicSettingsProvider.GetLogicSettings();
        if (settingsResult.IsSuccess)
        {
            settingsResult.Then(settings =>
                JsonSerializer.Serialize(outputStream, settings, options: jsonSerializerOptions));
            return (int)HttpStatusCode.OK;
        }

        throw new Exception(settingsResult.Error);
    }
}