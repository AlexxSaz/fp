using Autofac;
using TagCloudWebClient.UiActions;

namespace TagCloudWebClient;

public class TagCloudWebModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GetImageSettingsAction>().As<IApiAction>().SingleInstance();
        builder.RegisterType<GetPaletteSettingsAction>().As<IApiAction>().SingleInstance();
        builder.RegisterType<GetLogicSettingsAction>().As<IApiAction>().SingleInstance();
        builder.RegisterType<UpdateImageSettingsAction>().As<IApiAction>().SingleInstance();
        builder.RegisterType<UpdateLogicSettingsAction>().As<IApiAction>().SingleInstance();
        builder.RegisterType<GetWordsAction>().As<IApiAction>().SingleInstance();
        builder.RegisterType<App>().SingleInstance();
    }
}