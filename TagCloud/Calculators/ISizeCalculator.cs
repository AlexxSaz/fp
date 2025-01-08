using TagCloud.Infrastructure.Tags;

namespace TagCloud.Calculators;

public interface ISizeCalculator
{
    IReadOnlyCollection<IWordTag> Calculate(IEnumerable<string> words);
}