using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Entrypoints {
    public sealed class SingleServiceEntry : ServiceEntryPoint {
        private Thread OnConnectThread, HeaderParserThread;
        private List<TcpClient> ConnectingClients;
        public SingleServiceEntry(IPAddress address, int port) : base(address, port) {

        }

        public override bool OnStart() {
            bool started = Start();
            OnConnectThread = new Thread(() => OnConnect());
            HeaderParserThread = new Thread(() => ParseHeader());

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
                    ConnectingClients.Add(EntryTcpListener.AcceptTcpClient());
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
