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


            //new Thread(async () =>
            //{
            //    TcpClient client = new TcpClient();

            //    client.Connect("192.168.0.29", 80);
            //    NetworkStream stream = client.GetStream();

            //    while (true)
            //    {
            //        for (int i = 0; i < 20; i++)
            //        {
            //            await stream.WriteAsync(Encoding.UTF8.GetBytes("LMAO"), 0, Encoding.UTF8.GetBytes("LMAO").Length);
            //        }
            //        break;
            //    }

            //    await Task.Delay(30 * 1000);
            //    Console.WriteLine("stopping socket");
            //    client.Close();
            //}).Start();

            while(server.Active)
            {
                string console = Console.ReadLine().ToLower().Trim();
                switch(console)
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
