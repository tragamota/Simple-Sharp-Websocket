using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleWebsocket;
using SimpleWebsocket.Server;
using SimpleWebsocket.Server.Endpoints;

namespace UnitTest {
    class Program {
        private const string V = "info";

        public static async Task Main(string[] args) {
            Webserver server = new Webserver();
            ServiceEndPoint endpoint = new ServiceEndPoint(IPAddress.Any, 80)
            {
                //ServerCertificate = new TLSCertificate("find a chat.pfx", "AvansPassword")
            };
            server.AddEntryPoint(endpoint);

            await server.StartAsync();

            while(server.Active)
            {
                switch(Console.ReadLine().ToLower().Trim())
                {
                    case "info":
                        server.PrintInfo();
                        break;
                    case "stop":
                        server.Stop();
                        break;
                }
            }

            return;
        }
    }
}
