using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Simple_Sharp_Websocket
{
    public class ServerCertificate
    {
        private readonly X509Certificate2 _tlsCertificate;

        public ServerCertificate(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            _tlsCertificate = new X509Certificate2(path);
        }

        public async Task<Stream> AuthenticateClientAsServer(Stream connectionStream)
        {
            SslStream secureConnectionStream = new SslStream(connectionStream, false);

            try
            {
                await secureConnectionStream.AuthenticateAsServerAsync(_tlsCertificate);
            }
            catch (AuthenticationException ex)
            {
                secureConnectionStream = null;
            }

            return secureConnectionStream;
        }
    }
}