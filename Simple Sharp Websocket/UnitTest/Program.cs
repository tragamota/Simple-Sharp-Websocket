using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleWebsocket.Server;
using SimpleWebsocket.Server.Endpoints;

namespace UnitTest {
    class Program {
        public static async Task Main(string[] args) {
            Webserver server = new Webserver();
            server.AddEntryPoint(new ServiceEndPoint(IPAddress.Any, 80));

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

            while(true)
            { 
                await Task.Delay(1000);
            }

            return;
        }
    }
}
