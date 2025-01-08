using System.Drawing;
using System.Drawing.Imaging;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.TagCloudPainters;
using TagCloudConsoleClient.Options;
using TagCloudReader.Readers;

namespace TagCloudConsoleClient.Actions;

public class SaveImageAction(
    ITagCloudPainter tagCloudPainter,
    IImageSettingsProvider imageSettingsProvider,
    IPaletteProvider paletteProvider,
    IWordsReader wordsReader)
    : IConsoleAction
{
    public OptionType OptionType => OptionType.Save;

    public string Perform(IOption option)
    {
        var optionSettings = (SaveImageOption)option;
        var imageSettings = imageSettingsProvider.GetImageSettings();
        var palette = paletteProvider.GetPalette();

        var words = wordsReader.ReadFromTxt(optionSettings.InputTxtFile);
        var tagsInCloud = tagCloudPainter.GetTagsToPrintImage(words);

        const int rectangleOutline = 1;
        using var bitmap = new Bitmap(
            imageSettings.Width + rectangleOutline,
            imageSettings.Height + rectangleOutline);
        using var graphics = Graphics.FromImage(bitmap);
        var fontColor = palette.FontColor;
        var backgroundColor = palette.BackgroundColor;
        graphics.Clear(backgroundColor);
        using var brush = new SolidBrush(fontColor);

        foreach (var tag in tagsInCloud)
        {
            using var font = new Font(tag.Font.Family, tag.Font.Size);
            graphics.DrawString(tag.Value, font, brush, tag.Location.X, tag.Location.Y);
        }

        var path = optionSettings.OutputPngFile;
        bitmap.Save(path, ImageFormat.Png);
        return $"Картинка сохранена с именем {optionSettings.OutputPngFile}.";
    }
}