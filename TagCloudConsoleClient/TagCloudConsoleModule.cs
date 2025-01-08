using Autofac;
using TagCloudConsoleClient.Actions;
using TagCloudConsoleClient.Runners;

namespace TagCloudConsoleClient;

public class TagCloudConsoleModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SimpleAppRunner>().As<IAppRunner>().SingleInstance();
        builder.RegisterType<ImageSettingsAction>().As<IConsoleAction>().SingleInstance();
        builder.RegisterType<LogicSettingsAction>().As<IConsoleAction>().SingleInstance();
        builder.RegisterType<PaletteAction>().As<IConsoleAction>().SingleInstance();
        builder.RegisterType<SaveImageAction>().As<IConsoleAction>().SingleInstance();
        builder.RegisterType<ExitAction>().As<IConsoleAction>().SingleInstance();
    }
}