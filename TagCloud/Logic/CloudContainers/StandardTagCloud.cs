using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagCloud.Calculators;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudLayouts;
using TagCloud.WordHandlers;

namespace TagCloud.Logic.CloudContainers;

public class StandardTagCloud(
    ICloudLayout cloudLayout,
    IWordHandler[] wordHandlers,
    ISizeCalculator sizeCalculator,
    IImageSettingsProvider imageSettingsProvider)
    : ITagCloud
{
    private int _maxRight = 0;
    private int _maxBottom = 0;
    private int _minLeft = int.MaxValue;
    private int _minTop = int.MaxValue;
    private List<StandardWordTag> Tags { get; } = new();

    public int Width => Tags.Count == 0 ? 0 : _maxRight - _minLeft;

    public int Height => Tags.Count == 0 ? 0 : _maxBottom - _minTop;

    public int LeftBound => _minLeft;

    public int TopBound => _minTop;

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public IReadOnlyCollection<StandardWordTag> GetTags(IEnumerable<string> words)
    {
        var imageSettings = imageSettingsProvider.GetImageSettings();
        using var bitmap = new Bitmap(
            imageSettings.Width,
            imageSettings.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        var handledWords = wordHandlers.Aggregate(words, (current, handler) =>
            handler.Handle(current));
        var wordSizeDictionary = sizeCalculator.Calculate(handledWords);
        handledWords = handledWords.Distinct();
        foreach (var word in handledWords)
        {
            Tags.Add(GetNextTag(word, wordSizeDictionary[word], imageSettings, graphics));
            if (Width > imageSettings.Width || Height > imageSettings.Height)
                throw new ArgumentOutOfRangeException("Tagcloud large than the image size");
        }

        return Tags;
    }

    private StandardWordTag GetNextTag(string word, int wordSize, ImageSettings imageSettings, Graphics graphics)
    {
        var font = new Model.Font(imageSettings.FontFamily, wordSize);
        var frameSize = CalculateWordSize(graphics, word, font);
        var tagFrame = cloudLayout.PutNextRectangle(frameSize);

        _maxRight = Math.Max(_maxRight, tagFrame.Right);
        _maxBottom = Math.Max(_maxBottom, tagFrame.Bottom);
        _minLeft = Math.Min(_minLeft, tagFrame.Left);
        _minTop = Math.Min(_minTop, tagFrame.Top);

        var tagLocation = new Model.Point(tagFrame.X + imageSettings.Width / 2,
            tagFrame.Y + imageSettings.Height / 2);

        return new StandardWordTag(word, font, tagLocation);
    }

    private static Size CalculateWordSize(Graphics graphics, string word, Model.Font font)
    {
        var textSize = graphics.MeasureString(word, new Font(font.Family, font.Size));
        var viewWidth = (int)Math.Ceiling(textSize.Width);
        var viewHeight = (int)Math.Ceiling(textSize.Height);
        var viewSize = new Size(viewWidth, viewHeight);
        return viewSize;
    }
}