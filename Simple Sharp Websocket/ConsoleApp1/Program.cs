using System;
using System.Threading;
using Simple_Sharp_Websocket;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketListener listener = new SocketListener(null);

            listener.Start();

            while (listener.Running)
            {
                Thread.Sleep(1);
            }

        }
    }
}