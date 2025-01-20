using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.WordHandlers;

public class ExcludeWordHandler : IWordHandler
{
    public IEnumerable<string> Handle(IEnumerable<string> words, LogicSettings logicSettings) =>
        words.Where(word => !logicSettings.Exclusions.Contains(word));
}