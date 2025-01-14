using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class ImageSettingsProvider : IImageSettingsProvider
{
    private ImageSettings imageSettings = new();

    public ImageSettings GetImageSettings() => imageSettings;

    public void SetImageSettings(ImageSettings imageSettings) => this.imageSettings = imageSettings;
}