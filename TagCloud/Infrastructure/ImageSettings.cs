using System.Drawing;

namespace TagCloud.Infrastructure;

public record ImageSettings
{
    public int Width { get; init; } = 1000;
    public int Height { get; init; } = 1000;
    public FontFamily FontFamily { get; init; } = new("Arial");
    public int MaxFontSize { get; init; } = 36;
    public int MinFontSize { get; init; } = 12;
}