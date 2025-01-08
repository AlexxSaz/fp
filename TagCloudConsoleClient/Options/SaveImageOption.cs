using CommandLine;

namespace TagCloudConsoleClient.Options;

[Verb("save", HelpText = "Сохранение изображения с заданными настройками.")]
public class SaveImageOption : IOption
{
    public OptionType OptionType { get; } = OptionType.Save;
    
    [Option('i', "input-txt-file", HelpText = "Файл со словами.")]
    public string InputTxtFile { get; set; } = "aboutKonturWords.txt";
    [Option('o', "output-png-file", HelpText = "Файл с изображением.")]
    public string OutputPngFile { get; set; } = "aboutKontur.png";
}