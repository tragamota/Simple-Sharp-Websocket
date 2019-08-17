using System;
using System.IO;
using System.Text;

namespace SimpleSharp.Websocket
{
    internal class SocketWriter
    {
        public Stream BaseStream { get; private set; }

        public Encoding StreamEncoding { get; private set; }

        public SocketWriter(Stream baseStream) : this(baseStream, Encoding.UTF8) { }

        public SocketWriter(Stream baseStream, Encoding encoding)
        {
            this.BaseStream = baseStream;
            this.StreamEncoding = encoding;
        }

        public void WriteBytes(byte[] buffer)
        {
            if (buffer != null)
            {
                try
                {
                    BaseStream.Write(buffer, 0, buffer.Length);
                }
                catch (SystemException)
                {
                    //stream error
                }
            }
        }

        public async void WriteBytesAsync(byte[] buffer) {
            if(buffer != null) {
                try {
                    await BaseStream.WriteAsync(buffer, 0, buffer.Length);
                }
                catch(SystemException) {
                    //stream error 
                }
            }
        }

        public void WriteString(string sendString) {
            if(!string.IsNullOrEmpty(sendString)) {
                byte[] encodedBuffer = EncodeString(sendString);
                
                try {
                    BaseStream.Write(encodedBuffer, 0, encodedBuffer.Length);
                }
                catch(SystemException) {
                    //stream error
                }
            }
        }

        public void WriteStringAsync(string sendString) {
            if(!string.IsNullOrEmpty(sendString)) {
                byte[] encodedBuffer = EncodeString(sendString);

                try{
                    BaseStream.Write(encodedBuffer, 0, encodedBuffer.Length);
                }
                catch(SystemException) {
                    //stream error
                }
            }
        }

        private byte[] EncodeString(string unencodedString) {
            return StreamEncoding.GetBytes(unencodedString);
        }

        public void Close() {
            BaseStream.Close();
    
        }
    }
}