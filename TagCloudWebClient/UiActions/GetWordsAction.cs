using System.Net;
using System.Text.Json;
using ResultTools;
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
        var tagCloudResult = tagCloudCreator.Create(words);
        if (tagCloudResult.IsSuccess)
        {
            tagCloudResult.Then(tagCloud => JsonSerializer.Serialize(outputStream, tagCloud.Tags, options: jsonSerializerOptions));
            return (int)HttpStatusCode.OK;
        }
        JsonSerializer.SerializeAsync(outputStream, new ResultError(tagCloudResult.Error));
        return (int)HttpStatusCode.InternalServerError;
    }
}