using System.IO;
using System.IO.Pipelines;

namespace Simple_Sharp_Websocket
{
    public class ConnectionPipe
    {
        public IDuplexPipe SocketPipe { get; private set; }
        public IDuplexPipe ApplicationPipe { get; private set; }

        public ConnectionPipe(PipeOptions inputOptions = default, PipeOptions outputOptions = default)
        {
            Pipe input = new Pipe(inputOptions);
            Pipe output = new Pipe(outputOptions);
            
            CreatePipelinePair(input, output);
        }

        private void CreatePipelinePair(in Pipe inputPipe, in Pipe outputPipe)
        {
            SocketPipe = new TransportPipe(outputPipe.Reader, inputPipe.Writer);
            ApplicationPipe = new TransportPipe(inputPipe.Reader, outputPipe.Writer);
        }
    }
}