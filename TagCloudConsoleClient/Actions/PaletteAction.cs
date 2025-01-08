using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudConsoleClient.Options;

namespace TagCloudConsoleClient.Actions;

public class PaletteAction(IPaletteProvider paletteProvider) : IConsoleAction
{
    public OptionType OptionType => OptionType.Color;

    public string Perform(IOption option)
    {
        var optionSettings = (ColorSettingsOption)option;
        var palette = paletteProvider.GetPalette();
        var currentPalette = CreatePalette(optionSettings, palette);
        paletteProvider.SetPalette(currentPalette);
        return
            $"Цвета установлены. Цвет текста - {optionSettings.Font}, цвет фона - {optionSettings.Background}.";
    }

    private static Palette CreatePalette(ColorSettingsOption optionSettings, Palette palette) =>
        palette with
        {
            FontColor = optionSettings.Font, 
            BackgroundColor = optionSettings.Background
        };
}