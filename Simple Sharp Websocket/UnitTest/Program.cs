using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimpleWebsocket.Server;
using SimpleWebsocket.Server.Entrypoints;

namespace UnitTest {
    class Program {
        static void Main(string[] args) {
            Webserver server = new Webserver();
            server.AddEntryPoint(new SingleServiceEntry(IPAddress.Any, 80));
        }
    }
}
