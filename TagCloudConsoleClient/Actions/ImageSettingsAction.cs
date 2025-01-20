using ResultTools;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloudConsoleClient.Options;

namespace TagCloudConsoleClient.Actions;

public class ImageSettingsAction(IImageSettingsProvider imageSettingsProvider)
    : IConsoleAction
{
    public OptionType OptionType => OptionType.Image;

    public string Perform(IOption option)
    {
        var optionSettings = (ImageSettingsOption)option;
        var imageSettingsResult = imageSettingsProvider.GetImageSettings();

        if (!imageSettingsResult.IsSuccess) return $"Ошибка! {imageSettingsResult.Error}\nНастройки не применены.";

        imageSettingsResult.Then(imageSettings =>
        {
            var currentImageSettings = CreateImageSetting(optionSettings, imageSettings);
            imageSettingsProvider.SetImageSettings(currentImageSettings);
        });

        return $"Настройки изображения изменены.\n" +
               $"Ширина {optionSettings.Width}, высота {optionSettings.Height}.\n" +
               $"Максимальный размер текста {optionSettings.MaxFontSize}, минимальный {optionSettings.MinFontSize}.\n" +
               $"Тип шрифта {optionSettings.FontFamily.Name}";
    }

    private static ImageSettings CreateImageSetting(ImageSettingsOption imageSettingsOption,
        ImageSettings imageSettings) =>
        imageSettings with
        {
            FontFamily = imageSettingsOption.FontFamily,
            MaxFontSize = imageSettingsOption.MaxFontSize,
            MinFontSize = imageSettingsOption.MinFontSize,
            Width = imageSettingsOption.Width,
            Height = imageSettingsOption.Height
        };
}