using Autofac;
using TagCloud;
using TagCloudReader;
using TagCloudWebClient;

var builder = new ContainerBuilder();
builder.RegisterModule(new TagCloudReaderModule());
builder.RegisterModule(new TagCloudWebModule());
builder.RegisterModule(new TagCloudModule());

var app = builder.Build();
var appRunner = app.Resolve<App>();
await appRunner.Run();