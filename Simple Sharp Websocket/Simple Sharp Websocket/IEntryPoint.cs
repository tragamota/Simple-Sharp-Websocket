using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Entrypoints {
    interface IEntryPoint {
        bool OnStart();
        void OnStop();
    }
}
