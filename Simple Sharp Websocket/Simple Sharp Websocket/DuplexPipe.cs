using System.IO.Pipelines;
using System.Reflection.Emit;

namespace Simple_Sharp_Websocket
{
    public class TransportPipe : IDuplexPipe
    {
        public PipeReader Input { get; }
        public PipeWriter Output { get; }
        
        public TransportPipe(PipeReader reader, PipeWriter writer)
        {
            this.Input = reader;
            this.Output = writer;
        }
    }
}