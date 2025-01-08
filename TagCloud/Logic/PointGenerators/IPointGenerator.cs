using System.Drawing;

namespace TagCloud.Logic.PointGenerators;

public interface IPointGenerator
{
    PointGeneratorType PointGeneratorType { get; }
    public IEnumerable<Point> GeneratePoint();
}