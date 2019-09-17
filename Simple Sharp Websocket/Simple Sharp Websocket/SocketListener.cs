using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
        private ServerCertificate _listenerCertificate;

        private bool _running;
        private LinkedList<Task> _configurationTasks;
        private CancellationTokenSource _cancellationSource;
        
        private event EventHandler<OnNewClientEventArgs> OnClientEvent;

        public SocketListener(EventHandler<OnNewClientEventArgs> onConnectHandler, ServerCertificate certificate)
        {
            _listenerCertificate = certificate;
            _configurationTasks = new LinkedList<Task>();
            OnClientEvent = onConnectHandler;

        }

        public void Start()
        {
            if (_running) 
                return;

            _configurationTasks.Clear();
            _cancellationSource = new CancellationTokenSource();
            
            _listenerSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp)
            {
                DualMode = true
            };
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
                _configurationTasks.AddLast(HandleClientPreparation(await _listenerSocket.AcceptAsync()));
            }
        }

        private async Task HandleClientPreparation(Socket socket)
        {
            Stream connectionStream = await ConfigureStream(socket);

            if (connectionStream == Stream.Null) 
                return;
            
            ProduceNewClientEvent(socket, connectionStream);
        }

        private async Task<Stream> ConfigureStream(Socket socket)
        {
            Stream connectionStream = Stream.Null;
            
            socket.NoDelay = true;

            connectionStream = new NetworkStream(socket, false);

            if (_listenerCertificate != null)
                await _listenerCertificate.AuthenticateClientAsServer(connectionStream);
            
            return connectionStream;
        }

        private void ProduceNewClientEvent(Socket socket, Stream socketStream)
        {
            new SocketClient(socket, socketStream);
            OnClientEvent(this, new OnNewClientEventArgs()
            {
                NewClient = new SocketClient(socket, socketStream)
            });
        }

        public void Stop()
        {
            if (!_running)
                return;

            
            _running = false;
            _cancellationSource.Cancel();
            _listenerThread.Join();
        }
    }
}