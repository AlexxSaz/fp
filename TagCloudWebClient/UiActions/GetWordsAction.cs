using System.Net;
using System.Text.Json;
using ResultTools;
using TagCloud.Logic.CloudContainers;
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
        var wordsResult = reader.ReadFromString(wordContainer!.Words);
        Result<ITagCloud> cloudResult;
        if (wordsResult.IsSuccess)
            cloudResult = wordsResult.Then(words => SerializeAndGetCloud(words, outputStream));
        else
            throw new InvalidDataException(wordsResult.Error);

        if (cloudResult.IsSuccess)
            return (int)HttpStatusCode.OK;

        throw new InvalidDataException(cloudResult.Error);
    }

    private Result<ITagCloud> SerializeAndGetCloud(IEnumerable<string> words, Stream outputStream)
    {
        var tagCloudResult = tagCloudCreator.Create(words);

        return tagCloudResult.Then(tagCloud =>
        {
            JsonSerializer.Serialize(outputStream, tagCloud.Tags, options: jsonSerializerOptions);
            return tagCloud;
        });
    }
}