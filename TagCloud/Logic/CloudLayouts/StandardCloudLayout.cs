using System.Drawing;
using TagCloud.Extensions;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Logic.PointGenerators;

namespace TagCloud.Logic.CloudLayouts;

public class StandardCloudLayout : ICloudLayout
{
    private readonly IEnumerator<Point> _pointGeneratorIterator;

    private readonly List<Rectangle> _rectangles = [];

    public StandardCloudLayout(ILogicSettingsProvider logicSettingsProvider, IPointGenerator[] pointGenerators)
    {
        var logicSettings = logicSettingsProvider.GetLogicSettings();
        var pointGeneratorType = logicSettings.PointGeneratorType;
        var pointGenerator = GetPointGenerator(pointGeneratorType, pointGenerators);

        _pointGeneratorIterator = pointGenerator
            .GeneratePoint()
            .GetEnumerator();
    }

    private static IPointGenerator GetPointGenerator(PointGeneratorType pointGeneratorType, IPointGenerator[] pointGenerators)
    {
        var pointGenerator = pointGenerators.FirstOrDefault(pg => pg.PointGeneratorType == pointGeneratorType);

        if (pointGenerator == null)
            throw new ArgumentOutOfRangeException(nameof(pointGeneratorType),
                pointGeneratorType,
                "No such pointGenerator type.");
        return pointGenerator;
    }

    public Rectangle PutNextRectangle(Size size)
    {
        if (size.Width < 1 || size.Height < 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(size.Width)} and {nameof(size.Height)} should be greater than zero");

        Rectangle newRectangle;
        do newRectangle = GetNextRectangle(size);
        while (_rectangles.Any(rec => rec.IntersectsWith(newRectangle)));

        _rectangles.Add(newRectangle);
        return newRectangle;
    }

    private Rectangle GetNextRectangle(Size rectangleSize) =>
        new(GetNextRectangleCenter(rectangleSize), rectangleSize);

    private Point GetNextRectangleCenter(Size rectangleSize)
    {
        _pointGeneratorIterator.MoveNext();
        var rectangleCenter = ShiftRectangleLocationBy(rectangleSize);
        return _pointGeneratorIterator
            .Current
            .MoveTo(rectangleCenter);
    }

    private static Size ShiftRectangleLocationBy(Size rectangleSize) =>
        new(-rectangleSize.Width / 2, rectangleSize.Height / 2);
}