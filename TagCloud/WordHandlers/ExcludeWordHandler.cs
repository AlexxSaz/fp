using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.WordHandlers;

public class ExcludeWordHandler(ILogicSettingsProvider logicSettingsProvider) : IWordHandler
{
    public IEnumerable<string> Handle(IEnumerable<string> words)
    {
        var logicSettings = logicSettingsProvider.GetLogicSettings();
        return words.Where(word => !logicSettings.Exclusions.Contains(word));
    }
}