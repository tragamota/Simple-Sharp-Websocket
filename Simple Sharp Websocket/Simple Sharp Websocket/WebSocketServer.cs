using System;

namespace Simple_Sharp_Websocket
{
    public class WebSocketServer
    {
        public WebSocketConfiguration Configuration { get; }

        private SocketListener _listener;
        private ClientManager _clientManager;
        private SessionManager _sessionManager;

        public WebSocketServer(WebSocketConfiguration configuration)
        {
            this.Configuration = configuration;
            this._listener = new SocketListener((obj, args) => { _clientManager.ManageClient(args.NewClient); }, null);
            this._clientManager = new ClientManager();
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