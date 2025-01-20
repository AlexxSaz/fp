using System.Drawing;
using FluentAssertions;
using FluentAssertions.Execution;
using TagCloud.Extensions;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers;
using TagCloud.Logic.PointGenerators;

namespace TagCloudTests;

public class PointGeneratorShould
{
    private static readonly LogicSettingsProvider LogicSettingsProvider = new();
    private readonly Point defaultCenter = new();
    private readonly Random random = new();

    [TestCase(PointGeneratorType.Spiral)]
    [TestCase(PointGeneratorType.Astroid)]
    public void GetNewPoint_ReturnCenter_AfterFirstExecutionWith(PointGeneratorType pointGeneratorType)
    {
        var logicSettingsToSet = new LogicSettings();
        logicSettingsToSet = logicSettingsToSet with { PointGeneratorType = pointGeneratorType };
        LogicSettingsProvider.SetLogicSettings(logicSettingsToSet);
        var logicSettings = LogicSettingsProvider.GetLogicSettings().Value;
        var pointGenerator = new SpiralPointGenerator(logicSettings);
        using var newPointIterator = pointGenerator
            .GeneratePoint()
            .GetEnumerator();
        newPointIterator.MoveNext();
        var point = newPointIterator.Current;

        point
            .Should()
            .BeEquivalentTo(defaultCenter);
    }

    [TestCase(PointGeneratorType.Spiral)]
    [TestCase(PointGeneratorType.Astroid)]
    [Repeat(20)]
    public void GetNewPoint_ReturnPointWithGreaterRadius_WithPointGenerator(PointGeneratorType pointGeneratorType)
    {
        var newPointGenerator = GetPointGenerator(pointGeneratorType);
        var countOfPoints = random.Next(10, 200);
        var points = newPointGenerator
            .GeneratePoint()
            .Take(countOfPoints)
            .ToArray();

        var distances = points
            .Select(p => p.GetDistanceTo(defaultCenter))
            .ToArray();
        var angles = points
            .Select(p => Math.Atan2(p.Y - defaultCenter.Y, p.X - defaultCenter.X))
            .ToArray();

        using var _ = new AssertionScope();
        distances
            .Zip(distances.Skip(1), (a, b) => a <= b)
            .All(x => x)
            .Should()
            .BeTrue();
        angles
            .Zip(angles.Skip(1), (a, b) => a <= b || Math.Abs(a - b) < 0.1)
            .All(x => x)
            .Should()
            .BeTrue();
    }

    private static IPointGenerator GetPointGenerator(PointGeneratorType pointGeneratorType) =>
        pointGeneratorType switch
        {
            PointGeneratorType.Spiral => new SpiralPointGenerator(LogicSettingsProvider.GetLogicSettings().Value),
            PointGeneratorType.Astroid => new AstroidPointGenerator(LogicSettingsProvider.GetLogicSettings().Value),
            _ => throw new ArgumentOutOfRangeException(nameof(pointGeneratorType), pointGeneratorType, null)
        };
}