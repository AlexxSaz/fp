using System.Net;
using System.Text.Json;
using TagCloud.Infrastructure.Providers.Interfaces;

namespace TagCloudWebClient.UiActions;

public class GetPaletteSettingsAction(IPaletteProvider paletteProvider) : IApiAction
{
    public string Endpoint => "/palette";
        
    public string HttpMethod => "GET";
        
    public int Perform(Stream inputStream, Stream outputStream)
    {
        var palette = paletteProvider.GetPalette();
        JsonSerializer.Serialize(outputStream, palette);
        return (int)HttpStatusCode.OK;
    }
}