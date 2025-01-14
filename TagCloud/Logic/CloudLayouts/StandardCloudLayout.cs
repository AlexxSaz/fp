using System.Drawing;
using ResultTools;
using TagCloud.Extensions;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Logic.PointGenerators;

namespace TagCloud.Logic.CloudLayouts;

public class StandardCloudLayout : ICloudLayout
{
    private readonly IEnumerator<Point> pointGeneratorIterator;
    private readonly IPointGenerator[] pointGenerators;
    private readonly List<Rectangle> rectangles = [];

    public StandardCloudLayout(ILogicSettingsProvider logicSettingsProvider, IPointGenerator[] currentPointGenerators)
    {
        pointGenerators = currentPointGenerators;
        var logicSettings = logicSettingsProvider.GetLogicSettings();
        var pointGeneratorType = logicSettings.PointGeneratorType;
        var pointGenerator = GetPointGenerator(pointGeneratorType);

        pointGeneratorIterator = pointGenerator
            .GeneratePoint()
            .GetEnumerator();
    }

    private IPointGenerator GetPointGenerator(PointGeneratorType pointGeneratorType) =>
        pointGenerators.First(pointGenerator => pointGenerator.PointGeneratorType == pointGeneratorType);

    public Rectangle PutNextRectangle(Result<Size> size)
    {
        var newSize = size.Then(IsValidSize).GetValueOrThrow();

        Rectangle newRectangle;
        do newRectangle = GetNextRectangle(newSize);
        while (rectangles.Any(rec => rec.IntersectsWith(newRectangle)));

        rectangles.Add(newRectangle);
        return newRectangle;
    }

    private static Result<Size> IsValidSize(Size size) =>
        size.Width < 1 || size.Height < 1
            ? Result.Fail<Size>($"{nameof(size.Width)} and {nameof(size.Height)} should be greater than zero")
            : Result.Ok(size);

    private Rectangle GetNextRectangle(Size rectangleSize) =>
        new(GetNextRectangleCenter(rectangleSize), rectangleSize);

    private Point GetNextRectangleCenter(Size rectangleSize)
    {
        pointGeneratorIterator.MoveNext();
        var rectangleCenter = ShiftRectangleLocationBy(rectangleSize);
        return pointGeneratorIterator
            .Current
            .MoveTo(rectangleCenter);
    }

    private static Size ShiftRectangleLocationBy(Size rectangleSize) =>
        new(-rectangleSize.Width / 2, rectangleSize.Height / 2);
}