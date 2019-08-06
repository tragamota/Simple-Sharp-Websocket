using System.IO;
using System.Net.Sockets;

namespace SimpleSharp.Websocket
{
    internal class SocketClient
    {
        private TcpClient basesocket;
        private Stream baseStream;
        private SocketReader reader;
        private SocketWriter writer;
        public SocketClient(TcpClient baseSocket, Stream baseStream)
        {
            this.basesocket = baseSocket;
            this.baseStream = baseStream;
        }



    }
}