using System.Drawing;
using TagCloud.Extensions;
using TagCloud.Infrastructure;

namespace TagCloud.Logic.PointGenerators;

public class AstroidPointGenerator(LogicSettings logicSettings) : IPointGenerator
{
    private readonly Size centerPointSize = logicSettings.Center;
    private readonly double angleStep = logicSettings.AngleStep;
    private readonly double radiusStep = logicSettings.RadiusStep;
    public PointGeneratorType PointGeneratorType => PointGeneratorType.Astroid;

    public IEnumerable<Point> GeneratePoint()
    {
        var radius = 0d;
        var angle = 0d;

        while (true)
        {
            var newX = (int)(radius * Math.Pow(Math.Cos(angle), 3));
            var newY = (int)(radius * Math.Pow(Math.Sin(angle), 3));
            var newPoint = new Point(newX, newY).MoveTo(centerPointSize);

            radius += radiusStep;
            angle += angleStep;

            yield return newPoint;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}