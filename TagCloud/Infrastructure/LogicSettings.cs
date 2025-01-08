using System.Drawing;
using TagCloud.Logic.PointGenerators;

namespace TagCloud.Infrastructure;

public record LogicSettings
{
    public double RadiusStep { get; init; } = 0.01;
    public double AngleStep { get; init; } = 0.01;
    public Size Center { get; init; }
    public PointGeneratorType PointGeneratorType { get; init; } = PointGeneratorType.Spiral;
    public HashSet<string> Exclusions { get; init; } = new();
}