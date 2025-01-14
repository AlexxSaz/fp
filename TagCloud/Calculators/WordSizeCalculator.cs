using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Calculators;

public class WordSizeCalculator(IImageSettingsProvider imageSettingsProvider) : ISizeCalculator
{
    public IReadOnlyDictionary<string, int> Calculate(IEnumerable<string> words)
    {
        var dictionaryWithWordFrequency = GetDictionaryWithWordFrequency(words);
        var imageSettings = imageSettingsProvider.GetImageSettings();
        var maxFrequency = dictionaryWithWordFrequency.Values.Max();

        var result = new Dictionary<string, int>();
        foreach (var wordCountPair in dictionaryWithWordFrequency)
        {
            var normFreq = GetNormalizedFrequency(wordCountPair.Value, maxFrequency);
            var size = GetSize(normFreq, imageSettings.MaxFontSize, imageSettings.MinFontSize);
            result.Add(wordCountPair.Key, size);
        }

        return result;
    }

    private static Dictionary<string, int> GetDictionaryWithWordFrequency(IEnumerable<string> words)
    {
        var wordFrequencyDictionary = new Dictionary<string, int>();
        foreach (var word in words)
        {
            wordFrequencyDictionary.TryAdd(word, 0);
            wordFrequencyDictionary[word]++;
        }

        return wordFrequencyDictionary;
    }

    private static int GetSize(double normalizedFrequency, int maxSize, int minSize) =>
        (int)(normalizedFrequency * (maxSize - minSize) / 2 + minSize);

    private static double GetNormalizedFrequency(int count, int maxCount) =>
        Math.Log10(count + 1) / Math.Log10(maxCount + 1);
}