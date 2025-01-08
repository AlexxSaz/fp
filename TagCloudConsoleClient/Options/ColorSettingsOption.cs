using System.Drawing;
using CommandLine;

namespace TagCloudConsoleClient.Options;

[Verb("color", HelpText = "Настройка цвета")]
public class ColorSettingsOption : IOption
{
    [Option('f', "font", HelpText = "Цвет текста")]
    public Color Font { get; set; } = Color.Black;

    [Option('b', "background", HelpText = "Цвет заливки")]
    public Color Background { get; set; } = Color.White;

    public OptionType OptionType => OptionType.Color;
}