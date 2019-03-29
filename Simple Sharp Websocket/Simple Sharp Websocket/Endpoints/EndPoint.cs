using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints {
    public abstract class EndPoint : IEndPoint {
        #region Properties
        public IPEndPoint EndPointAddress { get; private set; }

        public bool Enabled { get; protected set; }
        public bool Active { get; protected set; }
        #endregion

        #region Constructor
        protected EndPoint(IPAddress address, int port) {
            if(port > IPEndPoint.MinPort && port <= IPEndPoint.MaxPort)
            {
                EndPointAddress = new IPEndPoint(address, port);
            }
        }
        #endregion

        #region API methods
        public abstract bool OnStart();
        public abstract void OnStop();

        protected abstract void Start(out bool success);
        protected abstract void OnConnect();
        protected abstract void Stop();
        #endregion
    }
}
