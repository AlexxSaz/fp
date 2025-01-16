using ResultTools;
using TagCloud.Logic.CloudContainers;

namespace TagCloud.Logic.CloudCreators;

public interface ITagCloudCreator
{
    Result<ITagCloud> Create(IEnumerable<string> words);
}