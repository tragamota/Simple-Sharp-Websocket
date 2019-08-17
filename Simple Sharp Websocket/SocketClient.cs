using System.IO;
using System.Net.Sockets;

namespace SimpleSharp.Websocket
{
    internal class SocketClient
    {
        private TcpClient baseSocket;
        private Stream baseStream;
        private SocketReader reader;
        private SocketWriter writer;
        
        public SocketClient(TcpClient baseSocket, Stream baseStream)
        {
            this.baseSocket = baseSocket;
            this.baseStream = baseStream;
            this.reader = new SocketReader(baseStream);
            this.writer = new SocketWriter(baseStream);
        }



        public void Close() {
            baseSocket.Close();
            baseStream.Close();
            reader.Close();
            writer.Close();
        }

    }
}