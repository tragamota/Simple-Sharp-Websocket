namespace SimpleWebsocket
{
    public interface IResponse
    {
        int StatusCode { get; }
        byte[] BinaryResponse();
    }
}