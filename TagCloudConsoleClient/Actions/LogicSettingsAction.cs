using ResultTools;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudConsoleClient.Options;

namespace TagCloudConsoleClient.Actions;

public class LogicSettingsAction(ILogicSettingsProvider logicSettingsProvider)
    : IConsoleAction
{
    public OptionType OptionType => OptionType.Logic;

    public string Perform(IOption option)
    {
        var optionSettings = (LogicSettingsOption)option;
        var logicSettingsResult = logicSettingsProvider.GetLogicSettings();
        if (!logicSettingsResult.IsSuccess) return $"Ошибка! {logicSettingsResult.Error}\nНастройки не применены.";
        
        logicSettingsResult.Then(logicSettings =>
        {
            var currentLogicSettings = CreateLogicSetting(optionSettings, logicSettings);
            logicSettingsProvider.SetLogicSettings(currentLogicSettings);
        });

        return $"Настройки логики изменены.\n" +
               $"Шаг угола {optionSettings.AngleStep}, шаг радиуса {optionSettings.RadiusStep}.\n" +
               $"Форма генерации точек: {optionSettings.PointGeneratorType}.\n" +
               $"Исключенные слова: {string.Join(", ", optionSettings.ExcludedWords.ToHashSet())}.\n";
    }

    private static LogicSettings CreateLogicSetting(LogicSettingsOption logicSettingsOption,
        LogicSettings logicSettings) =>
        logicSettings with
        {
            AngleStep = logicSettingsOption.AngleStep,
            RadiusStep = logicSettingsOption.RadiusStep,
            PointGeneratorType = logicSettingsOption.PointGeneratorType,
            Exclusions = logicSettingsOption.ExcludedWords.ToHashSet()
        };
}