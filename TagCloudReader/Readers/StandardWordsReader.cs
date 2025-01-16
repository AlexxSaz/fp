using ResultTools;

namespace TagCloudReader.Readers;

public class StandardWordsReader : IWordsReader
{
    private readonly string[] defaultWords = "Несколько дефолтных слов".Split();

    public Result<IEnumerable<string>> ReadFromTxt(string path) =>
        path
            .AsResult()
            .Then(IsStringValid)
            .Then(IsFileExists)
            .Then(x => GetResult(x, File.ReadAllLines));

    public Result<IEnumerable<string>> ReadFromString(string words) =>
        words
            .AsResult()
            .Then(IsStringValid)
            .Then(x => GetResult(x, s =>
                s.Split(["\n", "\r", "\r\n"], StringSplitOptions.RemoveEmptyEntries)));

    private static Result<string> IsFileExists(string path) =>
        File.Exists(path) ? path : Result.Fail<string>("File does not exist");

    private static Result<string> IsStringValid(string str) =>
        string.IsNullOrEmpty(str) ? Result.Fail<string>("String is empty") : str;

    private IEnumerable<string> GetResult(string str, Func<string, string[]> func)
    {
        var words = func(str);
        return words.Length == 0 ? defaultWords : words.Select(word => word);
    }
}