using System.IO;
using System.IO.Pipelines;

namespace Simple_Sharp_Websocket
{
    public struct ConnectionPipe
    {
        public IDuplexPipe SocketPipe { get; }
        public IDuplexPipe ApplicationPipe { get; }

        public ConnectionPipe(PipeOptions inputOptions = default, PipeOptions outputOptions = default)
        {
            Pipe input = new Pipe(inputOptions);
            Pipe output = new Pipe(outputOptions);
            
            CreatePipelinePair(input, output);
        }

        private void CreatePipelinePair(Pipe inputPipe, Pipe outputPipe)
        {
            TransportPipe SocketPipe =     
        }
    }
}