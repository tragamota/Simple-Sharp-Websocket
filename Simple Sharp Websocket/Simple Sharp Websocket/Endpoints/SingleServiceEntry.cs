using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints {
    public sealed class SingleServiceEntry : ServiceEntryPoint {
        private Thread OnConnectThread, HeaderParserThread;
        private List<TcpClient> unhandeldClients;
        public SingleServiceEntry(IPAddress address, int port) : base(address, port) {

        }

        public override bool OnStart() {
            bool started = Start();
            OnConnectThread = new Thread(() => OnConnect());
            HeaderParserThread = new Thread(() => ParseHeader());

            OnConnectThread.Start();
            HeaderParserThread.Start();

            return started;
        }

        public override void OnStop() {
            Stop();
            OnConnectThread.Join();
            HeaderParserThread.Join();

            OnConnectThread = null;
            HeaderParserThread = null;
        }

        protected override void OnConnect() {
            while(Active) {
                if(EntryTcpListener.Pending() && Active) {
                    unhandeldClients.Add(EntryTcpListener.AcceptTcpClient());
                }
                Thread.Yield();
            }
        }

        protected override void ParseHeader() {
            while(Active) {

            }
        }
    }
}
