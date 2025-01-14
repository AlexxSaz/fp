using System.Net;
using System.Text.Json;
using TagCloud.Logic.CloudCreators;
using TagCloudReader.Readers;
using TagCloudWebClient.JsonConverters;

namespace TagCloudWebClient.UiActions;

public class GetWordsAction(
    ITagCloudCreator tagCloudCreator,
    IWordsReader reader) : IApiAction
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { Converters = { new WordTagJsonConverter() } };

    public string Endpoint => "/getWords";

    public string HttpMethod => "POST";

    public int Perform(Stream inputStream, Stream outputStream)
    {
        var wordContainer = JsonSerializer.Deserialize<WordContainer>(inputStream);
        var words = reader.ReadFromString(wordContainer!.Words);
        var tagCloud = tagCloudCreator.Create(words);
        var tagsInCloud = tagCloud.Tags;

        JsonSerializer.Serialize(outputStream, tagsInCloud, options: jsonSerializerOptions);
        return (int)HttpStatusCode.OK;
    }
}