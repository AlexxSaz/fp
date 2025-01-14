using ResultTools;

namespace TagCloud.Infrastructure.Providers.Interfaces;

public interface ILogicSettingsProvider
{
    LogicSettings GetLogicSettings();
    void SetLogicSettings(Result<LogicSettings> currentLogicSettings);
}