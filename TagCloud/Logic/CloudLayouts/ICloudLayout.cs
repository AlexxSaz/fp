using System.Drawing;
using ResultTools;

namespace TagCloud.Logic.CloudLayouts;

public interface ICloudLayout
{
    public Result<Rectangle> PutNextRectangle(Size rectangleSize);
}