using ResultTools;

namespace TagCloudReader.Readers;

public interface IWordsReader
{
    IEnumerable<string> ReadFromTxt(Result<string> path);
    IEnumerable<string> ReadFromString(Result<string> words);
}