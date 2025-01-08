using System.Drawing;
using TagCloud.Extensions;
using TagCloud.Infrastructure;

namespace TagCloud.Logic.PointGenerators;

public class SpiralPointGenerator(LogicSettings logicSettings) : IPointGenerator
{
    private readonly Size _centerPointSize = logicSettings.Center;
    private readonly double _angleStep = logicSettings.AngleStep;
    private readonly double _radiusStep = logicSettings.RadiusStep;
    public PointGeneratorType PointGeneratorType => PointGeneratorType.Spiral;

    public IEnumerable<Point> GeneratePoint()
    {
        var radius = 0d;
        var angle = 0d;

        while (true)
        {
            var newX = (int)(radius * Math.Cos(angle));
            var newY = (int)(radius * Math.Sin(angle));
            var newPoint = new Point(newX, newY).MoveTo(_centerPointSize);

            radius += _radiusStep;
            angle += _angleStep;

            yield return newPoint;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}