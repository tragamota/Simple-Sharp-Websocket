using System;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using PipeOptions = System.IO.Pipes.PipeOptions;

namespace Simple_Sharp_Websocket
{
    public class NativeSocketClient : IDisposable
    {
        public Socket Socket { get; }
        public Stream SocketConnection { get; }

        public NativeSocketClient(Socket socket, Stream socketConnection)
        {
            this.Socket = socket;
            this.SocketConnection = socketConnection;
        }
        
        public void Dispose()
        {
            Socket?.Dispose();
            SocketConnection?.Dispose();
        }
    }
}