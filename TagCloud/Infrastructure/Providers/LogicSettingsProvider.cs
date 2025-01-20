using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Logic.PointGenerators;

namespace TagCloud.Infrastructure.Providers;

public class LogicSettingsProvider : ILogicSettingsProvider
{
    private Result<LogicSettings> logicSettings = new LogicSettings().AsResult();

    public Result<LogicSettings> GetLogicSettings() => logicSettings;

    public void SetLogicSettings(LogicSettings currentLogicSettings) =>
        logicSettings = currentLogicSettings
            .AsResult()
            .Then(CheckRadiusStepValue)
            .Then(CheckAngleStepValue);

    private static Result<LogicSettings> CheckRadiusStepValue(LogicSettings currentLogicSettings) =>
        currentLogicSettings.RadiusStep <= 0
            ? Result.Fail<LogicSettings>($"{nameof(currentLogicSettings.RadiusStep)} must be more than zero")
            : currentLogicSettings.AsResult();

    private static Result<LogicSettings> CheckAngleStepValue(LogicSettings currentLogicSettings) =>
        currentLogicSettings.AngleStep <= 0
            ? Result.Fail<LogicSettings>($"{nameof(currentLogicSettings.AngleStep)} must be more than zero")
            : currentLogicSettings.AsResult();
}