using Autofac;
using TagCloud;
using TagCloudConsoleClient;
using TagCloudConsoleClient.Runners;
using TagCloudReader;

var builder = new ContainerBuilder();
builder.RegisterModule(new TagCloudReaderModule());
builder.RegisterModule(new TagCloudConsoleModule());
builder.RegisterModule(new TagCloudModule());

var app = builder.Build();
var appRunner = app.Resolve<IAppRunner>();
appRunner.Run();