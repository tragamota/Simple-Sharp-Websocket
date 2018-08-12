using SimpleWebsocket.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server {
    public abstract class ServerSocket : IServerSocket {
        #region Variables
        private int activeServices, totalServices, totalEntryPoints;
        #endregion

        #region Properties
        public bool Active { get; protected set; }

        public int ActiveServices {
            get { return activeServices; }
            protected set { if (!Active) { activeServices = value; } }
        }

        public int TotalServices {
            get { return totalServices; }
            protected set { if (!Active) { totalServices = value; } }
        }

        public int TotalEntryPoints {
            get { return totalEntryPoints; }
            protected set { if(!Active) { totalEntryPoints = value; } }
        }
        #endregion

        #region Constructor
        public ServerSocket() {
            Active = false;
            activeServices = 0;
            totalServices = 0;
            totalEntryPoints = 0;
        }
        #endregion

        #region Methods
        public abstract bool Start();
        public abstract Task<bool> StartAsync();
        public abstract void Stop();
        public abstract Task StopAsync();
        public abstract string Info();
        #endregion
    }
}
