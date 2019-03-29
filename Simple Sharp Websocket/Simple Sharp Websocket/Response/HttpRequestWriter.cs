using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    internal class HttpResponseWriter
    {
        private Stream _clientStream;
        private BinaryWriter _responseWriter;

        public HttpResponseWriter(Stream clientStream)
        {
            _clientStream = clientStream;
            _responseWriter = new BinaryWriter(_clientStream, Encoding.UTF8, true);
        }

        public void SendResponse(IResponse response)
        {
            if(response != null)
            {
                _responseWriter.Write(response.ToString());
            }
        }

        public void Close()
        {
            _responseWriter.Close();
        }
    }
}