using TagCloud.Infrastructure;

namespace TagCloud.WordHandlers;

public interface IWordHandler
{
    IEnumerable<string> Handle(IEnumerable<string> words, LogicSettings logicSettings);
}