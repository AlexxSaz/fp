using System.Drawing;
using ResultTools;

namespace TagCloud.Logic.CloudLayouts;

public interface ICloudLayout
{
    public Rectangle PutNextRectangle(Result<Size> rectangleSize);
}