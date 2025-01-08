using Autofac;
using TagCloud.Calculators;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Providers;
using TagCloud.Infrastructure.Providers.Interfaces;
using TagCloud.Infrastructure.Tags;
using TagCloud.Logic.CloudContainers;
using TagCloud.Logic.CloudLayouts;
using TagCloud.Logic.PointGenerators;
using TagCloud.TagCloudPainters;
using TagCloud.WordHandlers;

namespace TagCloud;

public class TagCloudModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WordSizeCalculator>().As<ISizeCalculator>().SingleInstance();
        builder.RegisterType<StandardWordHandler>().As<IWordHandler>().SingleInstance();
        builder.RegisterType<ExcludeWordHandler>().As<IWordHandler>();
        builder.RegisterType<AstroidPointGenerator>().As<IPointGenerator>();
        builder.RegisterType<SpiralPointGenerator>().As<IPointGenerator>();
        
        builder.RegisterType<ImageSettingsProvider>().As<IImageSettingsProvider>().SingleInstance();
        builder.RegisterType<LogicSettingsProvider>().As<ILogicSettingsProvider>().SingleInstance();
        builder.RegisterType<PaletteProvider>().As<IPaletteProvider>().SingleInstance();
        
        builder.RegisterType<WordTagCloudPainter>().As<ITagCloudPainter>();
        builder.RegisterType<StandardTagCloud>().As<ITagCloud>();
        builder.RegisterType<StandardWordTag>().As<IWordTag>();
        builder.RegisterType<StandardCloudLayout>().As<ICloudLayout>();
        
        builder.RegisterType<ImageSettings>().SingleInstance();
        builder.RegisterType<Palette>().SingleInstance();
        builder.RegisterType<LogicSettings>().SingleInstance();
    }
}