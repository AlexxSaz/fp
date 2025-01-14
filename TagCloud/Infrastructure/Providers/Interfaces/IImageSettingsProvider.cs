using ResultTools;

namespace TagCloud.Infrastructure.Providers.Interfaces;

public interface IImageSettingsProvider
{
    ImageSettings GetImageSettings();
    void SetImageSettings(Result<ImageSettings> currentImageSettings);
}