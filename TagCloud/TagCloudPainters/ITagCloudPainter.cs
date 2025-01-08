using TagCloud.Infrastructure.Tags;

namespace TagCloud.TagCloudPainters;

public interface ITagCloudPainter
{
    IReadOnlyCollection<IWordTag> GetTagsToPrintImage(
        IEnumerable<string> wordTags);
}