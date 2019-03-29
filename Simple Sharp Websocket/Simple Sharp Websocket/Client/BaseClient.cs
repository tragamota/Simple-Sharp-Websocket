using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    public sealed class HttpClient
    {
        public bool Connected { get; set; }
        public bool Secure { get; set; }

        public Stream ClientStream { get; private set; }

        private readonly TcpClient _clientSocket;
        private HttpRequestReader _httpReader;
        private HttpResponseWriter _httpResponder;

        public HttpClient(ref TcpClient client, ref TLSCertificate certificate)
        {
            _clientSocket = client;

            Connected = true;
            Secure = false;
            ClientStream = _clientSocket.GetStream();

            if (certificate != null)
            {
                try
                {
                    ClientStream = Authenticate(certificate);
                }
                catch(AuthenticationException)
                {
                    Close();
                    return;
                }
            }

            _httpReader = new HttpRequestReader(ClientStream);
            _httpResponder = new HttpResponseWriter(ClientStream);
        }

        private Stream Authenticate(TLSCertificate certificate)
        {
            SslStream secureStream = new SslStream(ClientStream, false);
            secureStream.AuthenticateAsServer(certificate.Certificate, false, false);
     
            return secureStream;
        }

        public HttpRequest ReadHttpRequest()
        {
            return _httpReader.ReadHttpRequest();
        }

        public Task<HttpRequest> ReadHttpRequestAsync()
        {
            return Task.Run(() => ReadHttpRequest());
        }
       
        public void SendHttpResponse(IResponse httpResponse)
        {
            _httpResponder.SendResponse(httpResponse);
        }

        public Task SendHttpResponseAsync(IResponse httpResponse)
        {
            return Task.Run(() => SendHttpResponse(httpResponse));
        }

        public void Close()
        {
            Connected = false;

            _clientSocket.Close();
        }
    }
}
