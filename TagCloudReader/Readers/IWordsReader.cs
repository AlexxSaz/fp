namespace TagCloudReader.Readers;

public interface IWordsReader
{
    IEnumerable<string> ReadFromTxt(string path);
    IEnumerable<string> ReadFromString(string words);
}