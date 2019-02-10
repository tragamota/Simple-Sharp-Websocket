using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints {
    public sealed class ServiceEndPoint : EndPoint {
        private readonly TcpListener TcpSocket;
        private readonly Queue<TcpClient> IncomingClients;
        private Thread OnConnectThread;

        public ServiceEndPoint(IPAddress address, int port) : base(address, port) {
            TcpSocket = new TcpListener(EndPointAddress);
            IncomingClients = new Queue<TcpClient>(30);

            OnConnectThread = new Thread(OnConnect);
        }

        public override bool OnStart()
        {
            return Start();
        }

        public override void OnStop()
        {
            Stop();
        }

        protected async override void OnConnect()
        {
            while(Active)
            {
                TcpClient client = await TcpSocket.AcceptTcpClientAsync();
                Console.WriteLine("Nieuw verbinding");

                MemoryStream bufferStream = new MemoryStream(1024);
                bufferStream.

                Task task = Task.Run(async() =>
                {
                    byte[] buffer = new byte[1024];
                    int startPosition = 0;
                    int readBytes = 0;
                    NetworkStream stream = client.GetStream();
                    bool connected = true;
                    while (connected)
                    { 
                        try {
                            readBytes = await stream.ReadAsync(buffer, startPosition, 1024 - startPosition);
                            startPosition += readBytes;

                            if(readBytes == 0)
                            {
                                connected = false;
                            }
                        }
                        catch(IOException e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }



                        Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, startPosition));
                        if (startPosition > 0)
                        {
                            await stream.WriteAsync(Encoding.UTF8.GetBytes($@"HTTP/1.1 200 OK\r\n<html>
<head>
<title>
A Simple HTML Document
</title>
</head>
<body>
<p>This is a very simple HTML document</p>
<p>It only has two paragraphs</p>
</body>
</html>"), 0, Encoding.UTF8.GetBytes($@"HTTP/1.1 200 OK\r\n<html>
<head>
<title>
A Simple HTML Document
</title>
</head>
<body>
<p>This is a very simple HTML document</p>
<p>It only has two paragraphs</p>
</body>
</html>").Length);
                            client.Close();
                        }
                       
                        //Console.WriteLine(client.Connected);
                        //Console.WriteLine(startPosition);
                    }
                    
                    Console.WriteLine("Closed connection :)");
                });
            }
        }

        protected override bool Start()
        {
            bool success = false;
            if (!Active)
            {
                Active = true;
                success = true;
                TcpSocket.Start();
                OnConnectThread.Start();
            }
            return success;
        }

        protected override void Stop()
        {
            if(Active)
            {
                Active = false;
                TcpSocket.Stop();
            }
        }
    }
}
