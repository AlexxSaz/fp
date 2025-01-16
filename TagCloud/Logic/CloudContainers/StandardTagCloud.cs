using System.Drawing;
using ResultTools;
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
        cloudLayout
            .PutNextRectangle(frameSize)
            .Then(frame =>
            {
                maxRight = Math.Max(maxRight, frame.Right);
                maxBottom = Math.Max(maxBottom, frame.Bottom);
                minLeft = Math.Min(minLeft, frame.Left);
                minTop = Math.Min(minTop, frame.Top);
                return new Model.Point(frame.X + imageSettings.Width / 2,
                    frame.Y + imageSettings.Height / 2);
            })
            .Then(location => Tags.Add(new StandardWordTag(word, font, location)));
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