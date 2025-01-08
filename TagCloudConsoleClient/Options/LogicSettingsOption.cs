using CommandLine;
using TagCloud.Logic.PointGenerators;

namespace TagCloudConsoleClient.Options;

[Verb("logic", HelpText = "Настройка логики")]
public class LogicSettingsOption : IOption
{
    [Option('a', "angleStep", HelpText = "Шаг увеличения угла спирали.")]
    public double AngleStep { get; set; } = 0.01;

    [Option('r', "radiusStep", HelpText = "Шаг увеличения радиуса спирали.")]
    public double RadiusStep { get; set; } = 0.01;

    [Option('g', "generatorType", HelpText = "Тип генератора точек.")]
    public PointGeneratorType PointGeneratorType { get; set; } = PointGeneratorType.Spiral;
    
    [Option('e', "excludeWords", HelpText = "Исключить слова из облака")]
    public IEnumerable<string> ExcludedWords { get; set; } = [];

    public OptionType OptionType => OptionType.Logic;
}