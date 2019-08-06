using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSharp.Websocket 
{

    class SocketListener
    {
        public bool Running { get; private set; }
        private TcpListener socket;
        private StreamEstablisher establisher;
        private Thread listenerThread;
        public SocketListener(IPAddress address, int port)
        {
            this.socket = new TcpListener(address, port);
            this.establisher = new StreamEstablisher();
        }

        public void Start() 
        {
            if(!Running) {
                Running = !Running;
                
                listenerThread = new Thread(ListenForConnections);
                
                socket.Start();
                listenerThread.Start();
            }
        }

        public void Stop() 
        {
            if(Running) {
                Running = !Running;

                socket.Stop();
                listenerThread.Join();

                listenerThread = null;
            }
        }

        private async void ListenForConnections() 
        {
            List<Task> OnConnectTasks = new List<Task>();
            CancellationTokenSource ConnectCancelToken = new CancellationTokenSource();
            
            while(Running) {
                TcpClient incomingClient;

                try {
                    incomingClient = await socket.AcceptTcpClientAsync();
                }
                catch (Exception) {
                    break;
                }
                
                OnConnectTasks.Add(Task.Run(async () => {
                    await HandleClient(incomingClient);
                }, 
                ConnectCancelToken.Token).ContinueWith(task => {
                    OnConnectTasks.Remove(task);
                }));
            }

            ConnectCancelToken.Cancel();

            await Task.WhenAll(OnConnectTasks);
        }

        private async Task HandleClient(TcpClient client) {
            Stream socketStream = await establisher.EstablishStreamAsync(client);

            if(socketStream != null) {
                new SocketClient(client, socketStream);
            }

        }
    }
}