using ResultTools;

namespace TagCloudReader.Readers;

public interface IWordsReader
{
    Result<IEnumerable<string>> ReadFromTxt(string path);
    Result<IEnumerable<string>> ReadFromString(string words);
}