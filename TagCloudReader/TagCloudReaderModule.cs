using Autofac;
using TagCloudReader.Readers;

namespace TagCloudReader;

public class TagCloudReaderModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<StandardWordsReader>().As<IWordsReader>().SingleInstance();
    }
}