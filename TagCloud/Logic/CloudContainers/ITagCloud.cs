using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudLayouts;

namespace TagCloud.Logic.CloudContainers;

public interface ITagCloud
{
    IReadOnlyCollection<StandardWordTag> GetTags(IEnumerable<string> words);
}