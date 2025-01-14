using TagCloud.Logic.CloudContainers;

namespace TagCloud.Logic.CloudCreators;

public interface ITagCloudCreator
{
    ITagCloud Create(IEnumerable<string> words);
}