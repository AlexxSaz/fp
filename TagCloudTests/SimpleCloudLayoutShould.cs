﻿using System.Drawing;
using FluentAssertions;
using TagCloud.Extensions;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers;
using TagCloud.Logic.CloudLayouts;
using TagCloud.Logic.PointGenerators;

[assembly: Parallelizable(ParallelScope.Children)]

namespace TagCloudTests;

public class SimpleCloudLayoutShould
{
    private readonly Point defaultCenter = new();
    private readonly Random random = new();
    private readonly LogicSettingsProvider logicSettingsProvider = new();

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleWithExpectedLocation_AfterFirstExecution()
    {
        var expectedCenter = new Point(random.Next(-10, 10), random.Next(-10, 10));
        var logicSettingsToSet = new LogicSettings();
        logicSettingsToSet = logicSettingsToSet with { Center = new Size(expectedCenter) };
        logicSettingsProvider.SetLogicSettings(logicSettingsToSet);
        var logicSettings = logicSettingsProvider.GetLogicSettings().Value;
        IPointGenerator[] pointGenerators =
        [
            new SpiralPointGenerator(logicSettings),
            new AstroidPointGenerator(logicSettings)
        ];
        var rectangleSize = GenerateSize()
            .Take(1)
            .First();
        var cloudLayout = new StandardCloudLayout(logicSettingsProvider, pointGenerators);

        var actualRectangle = cloudLayout.PutNextRectangle(rectangleSize);

        actualRectangle
            .Value
            .GetCentralPoint()
            .Should()
            .BeEquivalentTo(expectedCenter);
    }

    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(0, 0)]
    public void PutNextRectangle_ResultIsNotSuccess_AfterExecutionWith(int width, int height)
    {
        var rectangleSize = new Size(width, height);
        var logicSettings = logicSettingsProvider.GetLogicSettings().Value;
        IPointGenerator[] pointGenerators =
        [
            new SpiralPointGenerator(logicSettings),
            new AstroidPointGenerator(logicSettings)
        ];
        var circularCloudLayout = new StandardCloudLayout(logicSettingsProvider, pointGenerators);

        var executePutNewRectangle = () =>
            circularCloudLayout
                .PutNextRectangle(rectangleSize);

        executePutNewRectangle.Invoke().IsSuccess.Should().BeFalse();
    }

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleThatNotIntersectsWithOther_AfterManyExecution()
    {
        var rectangleSizes = GenerateSize()
            .Take(random.Next(10, 200));
        var logicSettings = logicSettingsProvider.GetLogicSettings().Value;
        IPointGenerator[] pointGenerators =
        [
            new SpiralPointGenerator(logicSettings),
            new AstroidPointGenerator(logicSettings)
        ];
        var cloudLayout = new StandardCloudLayout(logicSettingsProvider, pointGenerators);

        var rectangles = rectangleSizes
            .Select(size => cloudLayout.PutNextRectangle(size))
            .ToArray();

        for (var i = 0; i < rectangles.Length; i++)
        for (var j = i + 1; j < rectangles.Length; j++)
            rectangles[i].Value.IntersectsWith(rectangles[j].Value).Should().BeFalse();
    }

    [Test]
    [Repeat(20)]
    public void PutNextRectangle_ReturnRectanglesInCircle_AfterManyExecution()
    {
        var rectangleSizes = GenerateSize()
            .Take(random.Next(100, 200));
        var logicSettings = logicSettingsProvider.GetLogicSettings().Value;
        IPointGenerator[] pointGenerators =
        [
            new SpiralPointGenerator(logicSettings),
            new AstroidPointGenerator(logicSettings)
        ];
        var circularCloudLayout = new StandardCloudLayout(logicSettingsProvider, pointGenerators);

        var rectanglesList = rectangleSizes
            .Select(rectangleSize => circularCloudLayout
                .PutNextRectangle(rectangleSize))
            .ToList();

        var circleRadius = rectanglesList
            .Select(rectangle => rectangle.Value.GetCentralPoint())
            .Max(pointOnCircle => pointOnCircle.GetDistanceTo(defaultCenter));

        var sumRectanglesSquare = rectanglesList.Sum(rectangle => rectangle.Value.Width * rectangle.Value.Height);
        var circleSquare = circleRadius * circleRadius * Math.PI;
        var precision = circleSquare * 0.375;

        circleSquare
            .Should()
            .BeApproximately(sumRectanglesSquare, precision);
    }

    private static IEnumerable<Size> GenerateSize()
    {
        var random = new Random();
        while (true)
        {
            var rectangleWidth = random.Next(10, 100);
            var rectangleHeight = random.Next(1, 25);
            yield return new Size(rectangleWidth, rectangleHeight);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}