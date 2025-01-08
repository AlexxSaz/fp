using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Infrastructure.Tags;
using Font = TagCloud.Model.Font;
using Point = TagCloud.Model.Point;

namespace TagCloud.Calculators;

public class WordSizeCalculator(IImageSettingsProvider imageSettingsProvider) : ISizeCalculator
{
    public IReadOnlyCollection<IWordTag> Calculate(IEnumerable<string> words)
    {
        var dictionaryWithWordFrequency = GetDictionaryWithWordFrequency(words);
        var imageSettings = imageSettingsProvider.GetImageSettings();
        var maxFrequency = dictionaryWithWordFrequency.Values.Max();

        return (from wordCountPair in dictionaryWithWordFrequency
                let normalizedFrequency = GetNormalizedFrequency(wordCountPair.Value, maxFrequency)
                let size = GetSize(normalizedFrequency, imageSettings.MaxFontSize, imageSettings.MinFontSize)
                select new StandardWordTag(wordCountPair.Key, new Font(imageSettings.FontFamily, size),
                    new Point(0, 0)))
            .Cast<IWordTag>().ToList();
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