using ResultTools;

namespace TagCloud.Infrastructure.Providers.Interfaces;

public interface IImageSettingsProvider
{
    Result<ImageSettings> GetImageSettings();
    void SetImageSettings(ImageSettings currentImageSettings);
}