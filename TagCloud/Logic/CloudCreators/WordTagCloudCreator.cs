using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ResultTools;
using TagCloud.Calculators;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Logic.CloudContainers;
using TagCloud.WordHandlers;

namespace TagCloud.Logic.CloudCreators;

public class WordTagCloudCreator(
    IImageSettingsProvider imageSettingsProvider,
    IWordHandler[] wordHandlers,
    ISizeCalculator sizeCalculator,
    Func<ITagCloud> tagCloudFactory) : ITagCloudCreator
{
    public Result<ITagCloud> Create(IEnumerable<string> words)
    {
        var tagCloud = tagCloudFactory();
        var imageSettings = imageSettingsProvider.GetImageSettings();
        return PrepareTagCloud(tagCloud, words, imageSettings).Then(cloud => cloud);
    }

    private Result<ITagCloud> PrepareTagCloud(ITagCloud tagCloud, IEnumerable<string> words,
        ImageSettings imageSettings) =>
        tagCloud
            .AsResult()
            .Then(cloud => FillCloud(cloud, words, imageSettings))
            .Then(cloud => CheckCloudSize(cloud, imageSettings));

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private ITagCloud FillCloud(ITagCloud tagCloud, IEnumerable<string> words, ImageSettings imageSettings)
    {
        using var bitmap = new Bitmap(
            imageSettings.Width,
            imageSettings.Height);
        using var graphics = Graphics.FromImage(bitmap);

        var handledWords = wordHandlers.Aggregate(words, (current, handler) =>
            handler.Handle(current));
        var wordSizeDictionary = sizeCalculator.Calculate(handledWords);
        handledWords = handledWords.Distinct();
        foreach (var word in handledWords)
            tagCloud.AddTag(word, wordSizeDictionary[word], imageSettings, graphics);

        return tagCloud;
    }

    private static Result<ITagCloud> CheckCloudSize(ITagCloud cloud, ImageSettings imageSettings) =>
        cloud.Height > imageSettings.Height
            ? Result.Fail<ITagCloud>("Image height is too small to place all tags")
            : cloud.Width > imageSettings.Width
                ? Result.Fail<ITagCloud>("Image width is too small to place all tags")
                : Result.Ok(cloud);
}