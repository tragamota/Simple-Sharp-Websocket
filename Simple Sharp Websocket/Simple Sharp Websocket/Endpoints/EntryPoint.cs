using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints {
    public abstract class EntryPoint : IEntryPoint {
        #region Properties
        public IPEndPoint EndPoint { get; private set; }
        #endregion

        #region Constructor
        protected EntryPoint(IPAddress address, int port) {
            if(port > IPEndPoint.MinPort && port <= IPEndPoint.MaxPort)
            EndPoint = new IPEndPoint(address, port);
        }
        #endregion
        
        #region API methods
        protected abstract bool Start();
        public abstract bool OnStart();
        protected abstract void OnConnect();
        public abstract void OnStop();
        protected abstract void Stop();
        #endregion
    }
}
