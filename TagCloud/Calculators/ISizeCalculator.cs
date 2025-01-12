using TagCloud.Infrastructure.Tags;

namespace TagCloud.Calculators;

public interface ISizeCalculator
{
    IReadOnlyDictionary<string, int> Calculate(IEnumerable<string> words);
}