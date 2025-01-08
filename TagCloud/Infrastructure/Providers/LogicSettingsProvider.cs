using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class LogicSettingsProvider : ILogicSettingsProvider
{
    private LogicSettings _logicSettings = new();

    public LogicSettings GetLogicSettings() => _logicSettings;

    public void SetLogicSettings(LogicSettings logicSettings) => _logicSettings = logicSettings;
}