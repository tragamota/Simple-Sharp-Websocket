using System.Collections.Generic;

namespace Simple_Sharp_Websocket
{
    internal class Router : IRouter
    {
        private List<IWebController> controllers;

        public Router()
        {
            controllers = new List<IWebController>();
        }
        
        public void AddService(IWebController controller)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveService(IWebController controller)
        {
            throw new System.NotImplementedException();
        }

        public void RouteConnection(SocketPipelineClient client, IHttpRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}