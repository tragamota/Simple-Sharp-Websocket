using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Simple_Sharp_Websocket
{
    public class SocketListener
    {
        public bool Running
        {
            get { return _running; }
            private set { _running = value; }
        }

        private Socket _listenerSocket;
        private Thread _listenerThread;
        private bool _running;

        private event EventHandler<OnNewClientEventArgs> _onClientEvent;

        public SocketListener(EventHandler<OnNewClientEventArgs> onConnectHandler)
        {
            _onClientEvent = onConnectHandler;
        }

        public void Start()
        {
            if (_running) 
                return;

            _listenerSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.DualMode = true;
            _listenerSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, 5000));
            _listenerSocket.Listen(1024);
            

            _listenerThread = new Thread(ListenForSockets);
            _listenerThread.Start();

            _running = true;
        }

        private async void ListenForSockets()
        {
            while (_running)
            {
                Socket acceptSocket = await _listenerSocket.AcceptAsync();
                acceptSocket.Blocking = false;
                
                Stream connectionStream = new NetworkStream(acceptSocket, true);
                
                //check for ssl certificate
                if (false)
                    Thread.Sleep(1);    
                    
                SocketClient client = new SocketClient(acceptSocket);

                _onClientEvent(this, new OnNewClientEventArgs()
                {
                    NewClient = client
                });
            }
        }

        public void Stop()
        {
            if (!_running)
                return;

            _running = false;
            _listenerThread.Join();
        }
    }
}