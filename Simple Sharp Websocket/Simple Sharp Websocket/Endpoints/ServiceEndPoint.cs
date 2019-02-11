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
        private TLSCertificate _serverCertificate;
        public TLSCertificate ServerCertificate {
            get { return _serverCertificate; }
            set { if(!Active) _serverCertificate = value; }
        }

        private IList<IWebService> _services;
        public IList<IWebService> Services { get; }
          

        private readonly TcpListener _tcpSocket;
        private Task _onConnectTask;

        public ServiceEndPoint(IPAddress address, int port) : base(address, port) {
            _tcpSocket = new TcpListener(EndPointAddress);
            _services = new List<IWebService>();
        }

        public void AddService(IWebService service)
        {
            _services.Add(service);
        }
        
        public void RemoveService(IWebService service)
        {
            _services.Remove(service);
        }

        public override bool OnStart()
        {
            bool success = false;
            Start(ref success);
            return success;
        }

        public override void OnStop()
        {
            Stop();
        }

        protected override void Start(ref bool success)
        {
            if (!Active)
            {
                Active = true;
                success = true;
                _tcpSocket.Start();
                _onConnectTask = Task.Run(() => OnConnect());
            }
        }

        protected async override void OnConnect()
        {
            List<BaseClient> clients = new List<BaseClient>();
            while (Active)
            {
                try
                {
                    TcpClient client = await _tcpSocket.AcceptTcpClientAsync();

                    BaseClient bClient = new BaseClient(ref client, ref _serverCertificate);
                    clients.Add(bClient);
                    Console.WriteLine(client.Client.LocalEndPoint);
                    Console.WriteLine(client.ReceiveBufferSize);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        protected async override void Stop()
        {
            if(Active)
            {
                Active = false;
                _tcpSocket.Stop();
                await _onConnectTask;
            }
        }
    }
}
