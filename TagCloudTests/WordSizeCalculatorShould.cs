using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentAssertions.Execution;
using TagCloud.Calculators;
using TagCloud.Infrastructure.Providers;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloudTests;

public class WordSizeCalculatorShould
{
    private readonly IImageSettingsProvider imageSettingsProvider = new ImageSettingsProvider();

    [Test]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void Calculate_ShouldReturnStringsWithSize_AfterExecutionWithOneWordCollection()
    {
        var imageSettings = imageSettingsProvider.GetImageSettings();
        var wordSizeCalculator = new WordSizeCalculator(imageSettingsProvider);
        var oneWordCollection = Enumerable.Repeat("ясно", 6);
        var expectedNumberOfWords = oneWordCollection.GroupBy(x => x).Count();

        var wordFrequencyDictionary = wordSizeCalculator.Calculate(oneWordCollection);

        using var _ = new AssertionScope();
        wordFrequencyDictionary.Count.Should().Be(expectedNumberOfWords);
        wordFrequencyDictionary.First().Key.Should().Be(oneWordCollection.FirstOrDefault());
        wordFrequencyDictionary.First().Value.Should()
            .BeInRange(imageSettings.MinFontSize, imageSettings.MaxFontSize);
    }
}