using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    public class TLSCertificate
    {
        public X509Certificate2 Certificate { get; private set; }

        public TLSCertificate(string path, string password)
        {
            try
            {
                Certificate = new X509Certificate2(path, password);
            }
            catch(CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public TLSCertificate(byte[] rawData, string password)
        {
            try
            {
                Certificate = new X509Certificate2(rawData, password);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static TLSCertificate Open(string path, string password)
        {
            return new TLSCertificate(path, password);
        }

        public static TLSCertificate Open(byte[] rawData, string password)
        {
            return new TLSCertificate(rawData, password);
        }
    }
}
