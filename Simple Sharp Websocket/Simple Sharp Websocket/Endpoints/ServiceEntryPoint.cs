using SimpleWebsocket.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints {
    public abstract class ServiceEntryPoint : TcpEntryPoint {
        #region Variables
        protected List<int> headerParsers;
        
        #endregion

        #region 
        public List<int> HeaderParsers {
            get { return headerParsers; }
        }
        #endregion

        internal ServiceEntryPoint(IPAddress address, int port) : base(address, port) {
            headerParsers = new List<int>(20);
        }

        protected abstract void ParseHeader();

        public void AddParser(int parser) {
            headerParsers.Add(parser);
        }

        public void RemoverParser(int parser) {
            headerParsers.Remove(parser);
        }
    }
}
