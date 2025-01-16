using System.Drawing;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Tags;

namespace TagCloud.Logic.CloudContainers;

public interface ITagCloud
{
    int Width { get; }
    int Height { get; }
    IReadOnlyCollection<StandardWordTag> Tags { get; }
    void AddTag(string word, int wordSize, ImageSettings imageSettings, Graphics graphics);
}