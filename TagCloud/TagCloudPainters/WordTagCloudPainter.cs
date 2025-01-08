using System.Drawing;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudContainers;
using Font = TagCloud.Model.Font;
using Point = TagCloud.Model.Point;

namespace TagCloud.TagCloudPainters;

public class WordTagCloudPainter(
    Func<ITagCloud> tagCloudFactory,
    IImageSettingsProvider imageSettingsProvider) : ITagCloudPainter
{
    public IReadOnlyCollection<IWordTag> GetTagsToPrintImage(
        IEnumerable<string> words)
    {
        var imageSettings = imageSettingsProvider.GetImageSettings();
        var tagCloud = tagCloudFactory();
        
        using var bitmap = new Bitmap(
            imageSettings.Width,
            imageSettings.Height);
        using var graphics = Graphics.FromImage(bitmap);
        var wordTags = tagCloud.GetTags(words);

        return (from tag in wordTags
            let font = tag.Font with { Family = imageSettings.FontFamily }
            let frame = tagCloud.CloudLayout.PutNextRectangle(CalculateWordSize(graphics, tag, font))
            let tagLocation = new Point(frame.X + imageSettings.Width / 2, frame.Y + imageSettings.Height / 2)
            select new StandardWordTag(tag.Value, font, tagLocation)).Cast<IWordTag>().ToList();
    }

    private static Size CalculateWordSize(Graphics graphics, IWordTag wordTag, Font font)
    {
        var textSize = graphics.MeasureString(wordTag.Value,
            new System.Drawing.Font(font.Family, font.Size));
        var viewWidth = (int)Math.Ceiling(textSize.Width);
        var viewHeight = (int)Math.Ceiling(textSize.Height);
        var viewSize = new Size(viewWidth, viewHeight);
        return viewSize;
    }
}