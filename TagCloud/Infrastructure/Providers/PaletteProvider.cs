using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class PaletteProvider : IPaletteProvider
{
    private Palette palette = new();

    public Palette GetPalette() => palette;

    public void SetPalette(Palette currentPalette) => palette = currentPalette;
}