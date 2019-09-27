using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Simple_Sharp_Websocket
{
    public class SocketPipelineClient : IDisposable
    {
        public bool Connected => _connected;
        public IDuplexPipe ApplicationPipe => _externalPipe;
        
        public event EventHandler DisconnectEvent;

        private readonly NativeSocketClient _nativeSocket;
        private readonly IDuplexPipe _internalPipe;
        private readonly IDuplexPipe _externalPipe;
        
        private bool _connected;
        
        public SocketPipelineClient(NativeSocketClient nativeSocket)
        {
            _nativeSocket = nativeSocket;
            _connected = true;
            
            var socketPipeline = new ConnectionPipe();
            _internalPipe = socketPipeline.SocketPipe;
            _externalPipe = socketPipeline.ApplicationPipe;
        }

        private void OnDisconnect()
        {
            if (_connected)
            {
                _connected = !_connected;
                DisconnectEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task FillPipeline()
        {
            PipeWriter incomingPipe = _internalPipe.Output;
            Stream dataStream = _nativeSocket.SocketConnection;

            while (_connected)
            {
                Memory<byte> poolMemory = incomingPipe.GetMemory(2048);

                try
                {
                    int totalReceivedBytes = await dataStream.ReadAsync(poolMemory);

                    if (totalReceivedBytes == 0)
                    {
                        //disconnect callback!
                        OnDisconnect();
                        break;
                    }

                    incomingPipe.Advance(totalReceivedBytes);
                }
                catch (IOException e)
                {
                    //log exception to user/console
                    Console.WriteLine(e.Message);
                }

                FlushResult pushResult = await incomingPipe.FlushAsync();
            }

            incomingPipe.Complete();
        }

        public async Task ConsumePipeline()
        {
            PipeReader outgoingPipe = _internalPipe.Input;
            Stream dataStream = _nativeSocket.SocketConnection;

            while (_connected)
            {
                ReadResult outgoingResult = await outgoingPipe.ReadAsync();
                ReadOnlySequence<byte> readOnlyData = outgoingResult.Buffer;

                try
                {
                    foreach (ReadOnlyMemory<byte> buffer in readOnlyData)
                    {
                        await dataStream.WriteAsync(buffer);
                    }
                }
                catch (IOException e)
                {
                    OnDisconnect();
                    break;
                }

                outgoingPipe.AdvanceTo(readOnlyData.End);
            }
            
            outgoingPipe.Complete();
        }

        public void Close()
        {
            _nativeSocket.Socket.Close();
            _nativeSocket.SocketConnection.Close();
            _internalPipe.Input.CancelPendingRead();
            _internalPipe.Input.Complete();
            _internalPipe.Output.CancelPendingFlush();
            _internalPipe.Output.Complete();
        }
        
        public void Dispose()
        {
            _nativeSocket?.Dispose();
        }
    }
}