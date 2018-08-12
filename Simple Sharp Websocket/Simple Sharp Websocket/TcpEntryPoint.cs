using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Entrypoints {
    public abstract class TcpEntryPoint : EntryPoint {
        private X509Certificate2 serverCertificate;

        public bool Active { get; protected set; }
        public TcpListener EntryTcpListener { get; private set; }

        public X509Certificate2 ServerCertificate {
            get { return serverCertificate; }
            set {
                if (!Active) {
                    if(value.HasPrivateKey) {
                        serverCertificate = value;
                    }
                    else {
                        Console.WriteLine("Certificate does not have a private key");
                    }
                }
            }
        }

        public TcpEntryPoint(IPAddress address, int port) : base(address, port) {
            EntryTcpListener = new TcpListener(EndPoint);
        }

        public TcpEntryPoint(IPAddress address, int port, X509Certificate2 certificate) : base(address, port) {
            EntryTcpListener = new TcpListener(EndPoint);
            ServerCertificate = certificate;
        }

        protected override bool Start() {
            bool success = false;
            if (!Active) {
                try {
                    EntryTcpListener.Start();
                    Active = true;
                    success = true;
                }
                catch (SocketException ex) {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else {
                Console.WriteLine("Entrypoint is already running");
            }

            return success;
        }

        protected override void Stop() {
            if (Active) {
                try {
                    EntryTcpListener.Stop();
                    Active = false;
                }
                catch (SocketException ex) {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
