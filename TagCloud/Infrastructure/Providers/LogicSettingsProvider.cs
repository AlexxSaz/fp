using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class LogicSettingsProvider : ILogicSettingsProvider
{
    private Result<LogicSettings> logicSettings = new LogicSettings().AsResult();

    public Result<LogicSettings> GetLogicSettings() => logicSettings;

    public void SetLogicSettings(LogicSettings currentLogicSettings) =>
        logicSettings = currentLogicSettings.AsResult().Then(CheckLogicSettingsValidity);

    private static Result<LogicSettings> CheckLogicSettingsValidity(LogicSettings currentLogicSettings) =>
        currentLogicSettings.AngleStep <= 0
            ? Result.Fail<LogicSettings>("Angle step should be more than zero")
            : currentLogicSettings.RadiusStep <= 0
                ? Result.Fail<LogicSettings>("Radius step should be more than zero")
                : Result.Ok(currentLogicSettings);
}