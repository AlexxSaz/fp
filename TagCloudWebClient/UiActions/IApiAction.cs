namespace TagCloudWebClient.UiActions;

public interface IApiAction
{
    string Endpoint { get; }

    string HttpMethod { get; }

    int Perform(Stream inputStream, Stream outputStream);
}