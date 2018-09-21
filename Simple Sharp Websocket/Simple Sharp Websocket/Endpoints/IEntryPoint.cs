using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints {
    public interface IEntryPoint {
        bool OnStart();
        void OnStop();
    }
}
