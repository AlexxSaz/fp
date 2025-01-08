using TagCloudConsoleClient.Options;

namespace TagCloudConsoleClient.Actions;

public interface IConsoleAction
{
    OptionType OptionType { get; }
    string Perform(IOption option);
}