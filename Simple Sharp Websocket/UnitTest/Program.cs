using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimpleWebsocket.Server;
using SimpleWebsocket.Server.Endpoints;

namespace UnitTest {
    class Program {
        static void Main(string[] args) {
            Webserver server = new Webserver();
            SingleServiceEntry endpoint = new SingleServiceEntry(IPAddress.Any, 80);
            server.AddEntryPoint(new SingleServiceEntry(IPAddress.Any, 80));
        }
    }
}
