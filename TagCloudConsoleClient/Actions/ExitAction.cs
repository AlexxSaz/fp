using TagCloudConsoleClient.Options;

namespace TagCloudConsoleClient.Actions;

public class ExitAction : IConsoleAction
{
    public OptionType OptionType => OptionType.Exit;

    public string Perform(IOption option)
    {
        Environment.Exit(0);
        return "Программа завершена.";
    }
}