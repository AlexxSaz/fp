using System.Drawing.Text;
using ResultTools;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class ImageSettingsProvider : IImageSettingsProvider
{
    private Result<ImageSettings> imageSettings = new ImageSettings().AsResult();

    public Result<ImageSettings> GetImageSettings() => imageSettings;

    public void SetImageSettings(ImageSettings currentImageSettings) =>
        imageSettings = currentImageSettings
            .AsResult()
            .Then(CheckImageSize)
            .Then(CheckFontFamily)
            .Then(CheckFontSize);

    private static Result<ImageSettings> CheckImageSize(ImageSettings currentImageSettings) =>
        currentImageSettings.Height <= 0
            ? Result.Fail<ImageSettings>("Height must be greater than zero")
            : currentImageSettings.Width <= 0
                ? Result.Fail<ImageSettings>("Width must be greater than zero")
                : Result.Ok(currentImageSettings);

    private static Result<ImageSettings> CheckFontFamily(ImageSettings currentImageSettings)
    {
        var fontCollection = new InstalledFontCollection();
        return fontCollection.Families.Any(family => currentImageSettings.FontFamily.Name == family.Name)
            ? Result.Ok(currentImageSettings)
            : Result.Fail<ImageSettings>("Font family not found");
    }

    private static Result<ImageSettings> CheckFontSize(ImageSettings currentImageSettings) =>
        currentImageSettings.MaxFontSize <= 0
            ? Result.Fail<ImageSettings>("Max font size must be greater than zero")
            : currentImageSettings.MinFontSize <= 0
                ? Result.Fail<ImageSettings>("Min font size must be greater than zero")
                : Result.Ok(currentImageSettings);
}