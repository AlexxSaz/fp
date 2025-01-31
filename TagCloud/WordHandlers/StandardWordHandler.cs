using TagCloud.Infrastructure;
using TagCloudReader.Readers;

namespace TagCloud.WordHandlers;

public class StandardWordHandler(IWordsReader reader) : IWordHandler
{
    private const string BoringWordsFilePath = "BoringWordsDictionary.txt";

    private HashSet<string> BoringWords =>
        reader.ReadFromTxt(BoringWordsFilePath).Value.ToHashSet();

    public IEnumerable<string> Handle(IEnumerable<string> words, LogicSettings logicSettings) =>
        words
            .Select(word => word.ToLower())
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !BoringWords.Contains(word));
}