using FluentAssertions;
using FluentAssertions.Execution;
using TagCloud.Infrastructure.Providers;
using TagCloud.WordHandlers;

namespace TagCloudTests;

public class ExcludeWordHandlerShould
{
    private static readonly HashSet<string> TestWords =
    [
        "он", "ты", "вы", "а", "в", "привет", "мир", "контур", "компания", "лучшая"
    ];

    [TestCase("привет")]
    [TestCase("мир")]
    [TestCase("лучшая")]
    public void Handle_ExcludeSelectedWords_AfterExecution(string excludedWord)
    {
        var logicSettingsProvider = new LogicSettingsProvider();
        var logicSettings = logicSettingsProvider.GetLogicSettings().Value;
        logicSettings.Exclusions.Add(excludedWord);
        logicSettingsProvider.SetLogicSettings(logicSettings);
        var wordHandler = new ExcludeWordHandler();

        var handledWords = wordHandler.Handle(TestWords, logicSettings).ToHashSet();

        using var _ = new AssertionScope();
        handledWords.Should().NotContain(excludedWord);
        handledWords.Count.Should().Be(TestWords.Count - 1, "because we excluded exactly one word");
    }
}