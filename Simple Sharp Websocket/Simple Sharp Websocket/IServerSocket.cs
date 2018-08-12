using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server
{
    interface IServerSocket {
        bool Start();
        Task<bool> StartAsync();
        void Stop();
        Task StopAsync();
    }
}
