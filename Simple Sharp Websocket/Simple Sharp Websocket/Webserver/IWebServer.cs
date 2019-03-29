using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server
{
    public interface IWebServer {
        bool Start();
        void Stop();
        Task<bool> StartAsync();
        Task StopAsync();
    }
}
