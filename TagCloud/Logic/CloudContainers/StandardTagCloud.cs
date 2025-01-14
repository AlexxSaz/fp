using System.Drawing;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudLayouts;

namespace TagCloud.Logic.CloudContainers;

public class StandardTagCloud(ICloudLayout cloudLayout) : ITagCloud
{
    private int maxRight;
    private int maxBottom;
    private int minLeft = int.MaxValue;
    private int minTop = int.MaxValue;
    public List<StandardWordTag> Tags { get; } = new();

    public int Width => Tags.Count == 0 ? 0 : maxRight - minLeft;

    public int Height => Tags.Count == 0 ? 0 : maxBottom - minTop;

    public void AddTag(string word, int wordSize, ImageSettings imageSettings, Graphics graphics)
    {
        var font = new Model.Font(imageSettings.FontFamily, wordSize);
        var frameSize = CalculateWordSize(graphics, word, font);
        var tagFrame = cloudLayout.PutNextRectangle(frameSize);

        maxRight = Math.Max(maxRight, tagFrame.Right);
        maxBottom = Math.Max(maxBottom, tagFrame.Bottom);
        minLeft = Math.Min(minLeft, tagFrame.Left);
        minTop = Math.Min(minTop, tagFrame.Top);

        var tagLocation = new Model.Point(tagFrame.X + imageSettings.Width / 2,
            tagFrame.Y + imageSettings.Height / 2);

        Tags.Add(new StandardWordTag(word, font, tagLocation));
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