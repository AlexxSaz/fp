using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class LogicSettingsProvider : ILogicSettingsProvider
{
    private LogicSettings logicSettings = new();

    public LogicSettings GetLogicSettings() => logicSettings;

    public void SetLogicSettings(Result<LogicSettings> currentLogicSettings) =>
        logicSettings = currentLogicSettings.Then(CheckLogicSettingsValidity).GetValueOrThrow();

    private static Result<LogicSettings> CheckLogicSettingsValidity(LogicSettings currentLogicSettings) =>
        currentLogicSettings.AngleStep <= 0
            ? Result.Fail<LogicSettings>("Angle step should be more than zero")
            : currentLogicSettings.RadiusStep <= 0
                ? Result.Fail<LogicSettings>("Radius step should be more than zero")
                : Result.Ok(currentLogicSettings);
}