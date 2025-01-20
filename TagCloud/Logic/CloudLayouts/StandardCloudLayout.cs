using System.Drawing;
using ResultTools;
using TagCloud.Extensions;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Logic.PointGenerators;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace TagCloud.Logic.CloudLayouts;

public class StandardCloudLayout : ICloudLayout
{
    private IEnumerator<Point> pointGeneratorIterator;
    private readonly IPointGenerator[] pointGenerators;
    private readonly List<Result<Rectangle>> rectanglesResults = [];

    public StandardCloudLayout(ILogicSettingsProvider logicSettingsProvider, IPointGenerator[] currentPointGenerators)
    {
        pointGenerators = currentPointGenerators;
        logicSettingsProvider
            .GetLogicSettings()
            .Then(logicSettings =>
            {
                var pointGeneratorType = logicSettings.PointGeneratorType;
                return GetPointGenerator(pointGeneratorType);
            })
            .Then(pointGenerator =>
            {
                pointGeneratorIterator = pointGenerator
                    .GeneratePoint()
                    .GetEnumerator();
            });
    }

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        var createdRectangle = rectangleSize
            .AsResult()
            .Then(IsValidSize)
            .Then(GetPlacedRectangle);

        rectanglesResults.Add(createdRectangle);
        return createdRectangle;
    }

    private IPointGenerator GetPointGenerator(PointGeneratorType pointGeneratorType) =>
        pointGenerators.First(pointGenerator => pointGenerator.PointGeneratorType == pointGeneratorType);

    private static Result<Size> IsValidSize(Size size) =>
        size.Width < 1 || size.Height < 1
            ? Result.Fail<Size>($"{nameof(size.Width)} and {nameof(size.Height)} should be greater than zero")
            : Result.Ok(size);

    private Result<Rectangle> GetPlacedRectangle(Size rectangleSize)
    {
        Result<Rectangle> newRectangle;
        do newRectangle = GetNextRectangle(rectangleSize);
        while (rectanglesResults.Any(rec => rec.Value.IntersectsWith(newRectangle.Value)));
        return newRectangle;
    }

    private Result<Rectangle> GetNextRectangle(Size rectangleSize) =>
        new Rectangle(GetNextRectangleCenter(rectangleSize), rectangleSize).AsResult();

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