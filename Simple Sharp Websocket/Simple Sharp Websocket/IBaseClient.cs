using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    interface IBaseClient
    {
        bool Connected { get; }
        bool Secure { get; }

        byte[] Read();
        void Send(byte[] binaryData);
        void Close();
    }
}
