using System.Drawing;
using System.Drawing.Imaging;
using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudContainers;
using TagCloud.Logic.CloudCreators;
using TagCloudConsoleClient.Options;
using TagCloudReader.Readers;

namespace TagCloudConsoleClient.Actions;

public class SaveImageAction(
    ITagCloudCreator tagCloudCreator,
    IImageSettingsProvider imageSettingsProvider,
    IPaletteProvider paletteProvider,
    IWordsReader wordsReader)
    : IConsoleAction
{
    public OptionType OptionType => OptionType.Save;

    public string Perform(IOption option)
    {
        var optionSettings = (SaveImageOption)option;
        var wordsResult = wordsReader.ReadFromTxt(optionSettings.InputTxtFile);

        Result<ITagCloud> cloudResult;
        if (wordsResult.IsSuccess)
            cloudResult = wordsResult.Then(tagCloudCreator.Create);
        else
            return $"Ошибка! {wordsResult.Error}\nКартинка не сохранена.";

        if (!cloudResult.IsSuccess) return $"Ошибка! {cloudResult.Error}\nКартинка не сохранена.";

        var bitmapResult = cloudResult.Then(tagCloud =>
        {
            var tagsInCloud = tagCloud.Tags;
            return GetBitmap(tagsInCloud);
        });
        
        if (!bitmapResult.IsSuccess) return $"Ошибка! {bitmapResult.Error}\nКартинка не сохранена.";

        bitmapResult.Then(bitmap =>
        {
            var path = optionSettings.OutputPngFile;
            bitmap.Save(path, ImageFormat.Png);
        });
        
        return $"Картинка сохранена с именем {optionSettings.OutputPngFile}";
    }

    private Result<Bitmap> GetBitmap(IReadOnlyCollection<StandardWordTag> tagsInCloud)
    {
        var imageSettingsResult = imageSettingsProvider.GetImageSettings();
        var palette = paletteProvider.GetPalette();
        const int rectangleOutline = 1;

        return imageSettingsResult.Then(imageSettings =>
        {
            var bitmap = new Bitmap(
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

            return bitmap;
        });
    }
}