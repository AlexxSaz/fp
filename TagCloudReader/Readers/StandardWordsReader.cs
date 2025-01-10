using ResultTools;

namespace TagCloudReader.Readers;

public class StandardWordsReader : IWordsReader
{
    private readonly IEnumerable<string> defaultWords = "Несколько дефолтных слов".Split();

    public Result<IEnumerable<string>> ReadFromTxt(Result<string> path) =>
        path
            .Then(x => IsStringValid(x))
            .Then(x => IsFileExists(x))
            .Then(x => GetResult(x, File.ReadAllLines));

    public Result<IEnumerable<string>> ReadFromString(Result<string> words) =>
        words
            .Then(x => IsStringValid(x))
            .Then(x => GetResult(x, s => s.Split(["\n", "\r", "\r\n"], StringSplitOptions.RemoveEmptyEntries)));

    private static Result<string> IsFileExists(Result<string> path) =>
        File.Exists(path.Value) ? path : Result.Fail<string>("File does not exist");

    private static Result<string> IsStringValid(Result<string> str) =>
        string.IsNullOrEmpty(str.Value) ? Result.Fail<string>("String is empty") : str;

    private IEnumerable<string> GetResult(string str, Func<string, string[]> func)
    {
        var words = func(str);
        return words.Length == 0 ? defaultWords : words.Select(word => word);
    }
}