using ResultTools;

namespace TagCloudReader.Readers;

public interface IWordsReader
{
    Result<IEnumerable<string>> ReadFromTxt(Result<string> path);
    Result<IEnumerable<string>> ReadFromString(Result<string> words);
}