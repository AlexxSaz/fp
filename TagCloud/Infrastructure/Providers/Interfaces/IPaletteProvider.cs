namespace TagCloud.Infrastructure.Providers.Interfaces;

public interface IPaletteProvider
{
    Palette GetPalette();
    void SetPalette(Palette palette);
}