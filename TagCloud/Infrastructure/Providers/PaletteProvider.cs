using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloud.Infrastructure.Providers;

public class PaletteProvider : IPaletteProvider
{
    private Palette _palette = new();

    public Palette GetPalette() => _palette;

    public void SetPalette(Palette palette) => _palette = palette;
}