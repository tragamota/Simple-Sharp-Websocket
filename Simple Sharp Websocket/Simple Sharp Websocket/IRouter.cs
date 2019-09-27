namespace Simple_Sharp_Websocket
{
    internal interface IRouter
    {
        void AddService(IWebController controller);
        void RemoveService(IWebController controller);
        void RouteConnection(SocketPipelineClient connection, IHttpRequest request);
    }
}