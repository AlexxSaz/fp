using ResultTools;

namespace TagCloud.Infrastructure.Providers.Interfaces;

public interface ILogicSettingsProvider
{
    Result<LogicSettings> GetLogicSettings();
    void SetLogicSettings(LogicSettings currentLogicSettings);
}