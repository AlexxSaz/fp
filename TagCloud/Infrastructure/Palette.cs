using System.Drawing;

namespace TagCloud.Infrastructure;

public record Palette
{
    public Color FontColor { get; init; } = Color.Black;
    public Color BackgroundColor { get; init; } = Color.Aquamarine;
}