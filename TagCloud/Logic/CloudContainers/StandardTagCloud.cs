using System.Diagnostics.CodeAnalysis;
using TagCloud.Calculators;
using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudLayouts;
using TagCloud.WordHandlers;

namespace TagCloud.Logic.CloudContainers;

public class StandardTagCloud(ICloudLayout cloudLayout, IWordHandler[] wordHandlers, ISizeCalculator sizeCalculator)
    : ITagCloud
{
    public ICloudLayout CloudLayout => cloudLayout;

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public IReadOnlyCollection<IWordTag> GetTags(IEnumerable<string> words)
    {
        var handledWords = words;
        foreach (var wordHandler in wordHandlers)
            handledWords = wordHandler.Handle(handledWords);
        return sizeCalculator.Calculate(handledWords);
    }
}