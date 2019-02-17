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
    public class BaseClient : IBaseClient
    {
        public bool Connected { get; private set; }
        public bool Secure { get; private set; }
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

            if (certificate != null)
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
            return Task.Run(async () =>
            {
                byte[] buffer = new byte[_readSize];
                while (Connected)
                {
                    int readBytes = await _clientConnectionStream.ReadAsync(buffer, 0, buffer.Length);
                    bool hasData = readBytes != 0;

                    if (hasData)
                    {
                        await _incomingDataStream.WriteAsync(buffer, 0, readBytes);
                        Console.WriteLine(_incomingDataStream.Length + "\t" + _incomingDataStream.Position);
                        await _incomingDataStream.ReadAsync(buffer, 0, buffer.Length);
                        Console.WriteLine(_incomingDataStream.Length + "\t" + _incomingDataStream.Position);
                        await _incomingDataStream.FlushAsync();
                        Console.WriteLine(_incomingDataStream.Length + "\t" + _incomingDataStream.Position);
                    }
                    else
                    {
                        Connected = false;
                        Close();
                    }
                }
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
                    bool hasData = readBytes != 0;

                    if (hasData)
                    {
                        try
                        {
                            await _clientConnectionStream.WriteAsync(writeBuffer, 0, readBytes);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Close();
                        }
                    }
                    await Task.Yield();
                    await Task.Delay(100);
                }
            });
        }

        public byte[] Read()
        {
            byte[] buffer = new byte[0];
            while (Connected)
            {
                try
                {
                    if (_incomingDataStream.Length > 0)
                    {
                        buffer = new byte[_incomingDataStream.Length];
                        _incomingDataStream.Read(buffer, 0, buffer.Length);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return buffer;
        }

        public void Send(byte[] binaryData)
        {
            try
            {
                _outgoingDataStream.Write(binaryData, 0, binaryData.Length);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Close()
        {
            Connected = false;
            _outgoingDataStream.Close();
            _incomingDataStream.Close();
            _clientConnectionStream.Close();
            _clientSocket.Close();
        }
    }
}
