using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SimpleSharp.Websocket
{
    internal class StreamEstablisher
    {
        private X509Certificate2 sslCertificate;

        public StreamEstablisher()
        {
            this.sslCertificate = null;
        }

        public StreamEstablisher(X509Certificate2 sslCertificate) {
            this.sslCertificate = sslCertificate;
        }

        public Stream EstablishStream(ref TcpClient client) {
            Stream socketStream = client.GetStream();
            
            if(sslCertificate != null) {
                SslStream secureStream = new SslStream(socketStream, false);

                try {
                    AuthenticateStream(ref secureStream);
                }
                catch(AuthenticationException) {
                    secureStream.Close();
                    secureStream = null;
                }

                socketStream = secureStream;
            }

            return socketStream;
        }

        public async Task<Stream> EstablishStreamAsync(TcpClient client) {
            Stream socketStream = client.GetStream();

            if(sslCertificate != null) {
                SslStream secureStream = new SslStream(socketStream, false);

                try {
                    await AuthenticateStreamAsync(secureStream);
                }
                catch(AuthenticationException) {
                    secureStream.Close();
                    secureStream = null;
                }

                socketStream = secureStream;
            }

            return socketStream;
        }

        private void AuthenticateStream(ref SslStream secureStream) {
            secureStream.AuthenticateAsServer(sslCertificate);
        }

        private async Task AuthenticateStreamAsync(SslStream secureStream) {
            await secureStream.AuthenticateAsServerAsync(sslCertificate);
        }
    }
}