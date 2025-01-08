using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudLayouts;

namespace TagCloud.Logic.CloudContainers;

public interface ITagCloud
{
    ICloudLayout CloudLayout { get; }
    IReadOnlyCollection<IWordTag> GetTags(IEnumerable<string> words);
}