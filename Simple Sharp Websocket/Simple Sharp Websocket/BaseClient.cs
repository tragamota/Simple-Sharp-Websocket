using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    public class BaseClient
    {
        public bool Connected { get; private set; }
        private readonly int _initialMemorySize = 4096;
        private readonly int _readSize = 8192;
        private readonly int _writeSize = 4096;

        private readonly TcpClient _clientSocket;
        private readonly Stream _clientConnectionStream;
        private readonly MemoryStream _incomingDataStream;
        private readonly MemoryStream _outgoingDataStream;

        private readonly Task _onReadTask, _onWriteTask;

        public BaseClient(ref TcpClient client, ref TLSCertificate certificate)
        {
            Connected = true;
            _clientSocket = client;
            _incomingDataStream = new MemoryStream(_initialMemorySize);
            _outgoingDataStream = new MemoryStream(_initialMemorySize);

            if(certificate != null)
            {
                _clientConnectionStream = new SslStream(_clientSocket.GetStream(), false);
                ValidateServerCertificate(ref certificate);
            }
            else
            {
                _clientConnectionStream = _clientSocket.GetStream();
            }

            _onReadTask = NetworkReadTask();
            _onWriteTask = NetworkWriteTask();
        }

        private void ValidateServerCertificate(ref TLSCertificate certificate) 
        {
            SslStream authenticationStream = (SslStream)_clientConnectionStream;
            authenticationStream.AuthenticateAsServer(certificate.Certificate, false, false);
        }

        private Task NetworkReadTask()
        {
            return Task.Run(async () => {
                byte[] buffer = new byte[_readSize]; 
                while (Connected)
                {
                    int readBytes = await _clientConnectionStream.ReadAsync(buffer, 0, buffer.Length);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, readBytes));
                    if (readBytes > 0) {
                        await _incomingDataStream.WriteAsync(buffer, 0, readBytes);
                    }
                    else
                    {
                        Connected = false;
                        continue;
                    }
                }
               
                Console.WriteLine("Closing Task " + Task.CurrentId);
                _incomingDataStream.Close();
                _clientSocket.Close();
            });
        }

        private Task NetworkWriteTask()
        {
            return Task.Run(async () =>
            {
                byte[] writeBuffer = new byte[_writeSize];
                while (Connected)
                {
                    int readBytes = await _outgoingDataStream.ReadAsync(writeBuffer, 0, writeBuffer.Length);
                    
                    if(readBytes > 0) {
                        try
                        {
                            await _clientConnectionStream.WriteAsync(writeBuffer, 0, readBytes);
                        }
                        catch (Exception ex)
                        {
                            Connected = false;
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }
                _outgoingDataStream.Close();
            });
        }
    }
}
