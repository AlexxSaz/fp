﻿using System.Net;
using System.Text.Json;
using TagCloud.Logic.CloudContainers;
using TagCloudReader.Readers;
using TagCloudWebClient.JsonConverters;

namespace TagCloudWebClient.UiActions;

public class GetWordsAction(
    ITagCloud tagCloud,
    IWordsReader reader) : IApiAction
{
    private readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { Converters = { new WordTagJsonConverter() } };

    public string Endpoint => "/getWords";

    public string HttpMethod => "POST";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var wordContainer = JsonSerializer.Deserialize<WordContainer>(inputStream);
        var words = reader.ReadFromString(wordContainer!.Words);
        var tagsInCloud = tagCloud.GetTags(words.GetValueOrThrow());

        JsonSerializer.Serialize(outputStream, tagsInCloud, options: _jsonSerializerOptions);
        return (int)HttpStatusCode.OK;
    }
}