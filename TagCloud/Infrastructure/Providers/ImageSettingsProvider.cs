using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class ImageSettingsProvider : IImageSettingsProvider
{
    private ImageSettings _imageSettings = new();

    public ImageSettings GetImageSettings() => _imageSettings;

    public void SetImageSettings(ImageSettings imageSettings) => _imageSettings = imageSettings;
}