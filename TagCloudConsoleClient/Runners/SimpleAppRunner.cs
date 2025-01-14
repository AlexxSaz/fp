using CommandLine;
using TagCloudConsoleClient.Actions;
using TagCloudConsoleClient.Options;

namespace TagCloudConsoleClient.Runners;

public class SimpleAppRunner : IAppRunner
{
    private readonly IReadOnlyDictionary<OptionType, IConsoleAction> routeActions;

    public SimpleAppRunner(IEnumerable<IConsoleAction> actions)
    {
        var actionsArray = actions.ToArray();
        routeActions = actionsArray.ToDictionary(action => action.OptionType, action => action);
    }
    
    private readonly Type[] optionsTypes =
    [
        typeof(ImageSettingsOption),
        typeof(LogicSettingsOption),
        typeof(ColorSettingsOption),
        typeof(SaveImageOption),
        typeof(ExitOption)
    ];

    public void Run()
    {
        while (true)
        {
            var input = Console.ReadLine();
            var args = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];
            Parser.Default.ParseArguments(args, optionsTypes)
                .WithParsed<IOption>(Perform);
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    private void Perform(IOption option)
    {
        var action = routeActions[option.OptionType];
        Console.WriteLine(action.Perform(option));
    }
}