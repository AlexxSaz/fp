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
    ILogicSettingsProvider logicSettingsProvider,
    IWordHandler[] wordHandlers,
    ISizeCalculator sizeCalculator,
    Func<ITagCloud> tagCloudFactory) : ITagCloudCreator
{
    public Result<ITagCloud> Create(IEnumerable<string> words)
    {
        var tagCloud = tagCloudFactory();
        var imageSettingsResult = imageSettingsProvider.GetImageSettings();
        return imageSettingsResult.Then(imageSettings =>
            PrepareTagCloud(tagCloud, words, imageSettings).Then(cloud => cloud));
    }

    private Result<ITagCloud> PrepareTagCloud(ITagCloud tagCloud, IEnumerable<string> words,
        ImageSettings imageSettings) =>
        tagCloud
            .AsResult()
            .Then(cloud => FillCloud(cloud, words, imageSettings))
            .Then(cloud => CheckCloudSize(cloud, imageSettings));

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private Result<ITagCloud> FillCloud(ITagCloud tagCloud, IEnumerable<string> words, ImageSettings imageSettings)
    {
        var logicSettingsResult = logicSettingsProvider.GetLogicSettings();

        return logicSettingsResult.Then(logicSettings =>
        {
            using var bitmap = new Bitmap(
                imageSettings.Width,
                imageSettings.Height);
            using var graphics = Graphics.FromImage(bitmap);
            var handledWords = wordHandlers.Aggregate(words, (current, handler) =>
                handler.Handle(current, logicSettings));
            var wordSizeDictionary = sizeCalculator.Calculate(handledWords, imageSettings);
            handledWords = handledWords.Distinct();
            foreach (var word in handledWords)
                tagCloud.AddTag(word, wordSizeDictionary[word], imageSettings, graphics);

            return tagCloud;
        });
    }

    private static Result<ITagCloud> CheckCloudSize(ITagCloud cloud, ImageSettings imageSettings) =>
        cloud.Height > imageSettings.Height
            ? Result.Fail<ITagCloud>("Image height is too small to place all tags")
            : cloud.Width > imageSettings.Width
                ? Result.Fail<ITagCloud>("Image width is too small to place all tags")
                : Result.Ok(cloud);
}