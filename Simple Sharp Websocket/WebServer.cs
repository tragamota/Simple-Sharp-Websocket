using System;
using System.Net;

namespace SimpleSharp.Websocket
{
    public class WebServer
    {
        private WebsocketConfiguration configurations;
        private SocketListener socketListener;
        public WebsocketConfiguration Configurations
        {
            get { return configurations;}
            set 
            { 
                if(!socketListener.Running) 
                    configurations = value;
            }
        }
          
        public WebServer(IPAddress address, int port) 
        {
            this.socketListener = new SocketListener(address, port);
        }

        public void Start() 
        {
            socketListener.Start();
        }

        public void Stop()
        {
            socketListener.Stop();
        }
    }
}
