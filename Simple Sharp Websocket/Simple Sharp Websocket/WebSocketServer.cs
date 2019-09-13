
using System;
namespace Simple_Sharp_Websocket
{
    public class WebSocketServer
    {
        public WebSocketConfiguration Configuration { get; }
        
        private SocketListener _listener;

        public WebSocketServer(WebSocketConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}