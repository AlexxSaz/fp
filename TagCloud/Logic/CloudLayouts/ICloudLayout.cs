using System.Drawing;

namespace TagCloud.Logic.CloudLayouts;

public interface ICloudLayout
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}