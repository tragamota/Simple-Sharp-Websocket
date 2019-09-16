using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using PipeOptions = System.IO.Pipes.PipeOptions;

namespace Simple_Sharp_Websocket
{
    public class SocketClient
    {
        private readonly Socket Socket;
        private readonly Stream SocketConnection;
        
        private Pipe _pipeline;
        public PipeReader PipeReader;
        public PipeWriter PipeWriter;
        
        public SocketClient(Socket socket, Stream socketConnection)
        {
            this.Socket = socket;
            this.SocketConnection = socketConnection;
            
            this._pipeline = new Pipe();
            this.PipeReader = _pipeline.Reader;
            this.PipeWriter = _pipeline.Writer;
        }

        public async Task FeedPipeline()
        {
            DuplexPipe.Create
        }

        public async Task ConsumePipeline()
        {
            
        }
        
        public void Close()
        {
            Socket.Close();
        }
    }
}