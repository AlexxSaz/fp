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
            .Then(CheckAngleStepValue)
            .Then(CheckPointGeneratorType);

    private static Result<LogicSettings> CheckRadiusStepValue(LogicSettings currentLogicSettings) =>
        currentLogicSettings.RadiusStep <= 0
            ? Result.Fail<LogicSettings>($"{nameof(currentLogicSettings.RadiusStep)} should be greater than zero")
            : currentLogicSettings.AsResult();

    private static Result<LogicSettings> CheckAngleStepValue(LogicSettings currentLogicSettings) =>
        currentLogicSettings.AngleStep <= 0
            ? Result.Fail<LogicSettings>($"{nameof(currentLogicSettings.AngleStep)} should be greater than zero")
            : currentLogicSettings.AsResult();

    private static Result<LogicSettings> CheckPointGeneratorType(LogicSettings currentLogicSettings) =>
        Enum.IsDefined(typeof(PointGeneratorType), currentLogicSettings.PointGeneratorType)
            ? currentLogicSettings.AsResult()
            : Result.Fail<LogicSettings>($"Unknown point generator type.\n" +
                                         $"Point generator type should be one of following types: " +
                                         $"{Enum.GetValues(typeof(PointGeneratorType))}");
}