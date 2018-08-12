using SimpleWebsocket.Server.Entrypoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server {
    public sealed class Webserver : ServerSocket {
        #region Properties
        public List<EntryPoint> EntryPoints { get; }
        #endregion

        public Webserver() {
            EntryPoints = new List<EntryPoint>(20);
        }

        #region Methods
        public override bool Start() {
            bool result = false;
            if (EntryPoints.Any()) {
                result = true;
            }
            StartEntryPoints();
            return result;
        }

        public async override Task<bool> StartAsync() {
            return await Task.Run(() => Start());
        }

        public override void Stop() {
            foreach(EntryPoint entry in EntryPoints) {
                entry.OnStop();
            }
        }

        public async override Task StopAsync() {
            await Task.Run(() => Stop());
        }

        public override string Info() {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine(Active ? "The server is currently running..." : "The server is currently not running...");
            infoBuilder.AppendLine("This server contains " + TotalEntryPoints + " entrypoints");
            infoBuilder.AppendLine("There are at the moment " + ActiveServices + "/" + TotalServices + " running");
            infoBuilder.AppendLine("Wanna have info about a service, grab that service and ask for info");
            return infoBuilder.ToString();
        }

        public void AddEntryPoint(EntryPoint entry) {
            if(!Active) {
                if (entry != null) {
                    EntryPoints.Add(entry);
                    TotalEntryPoints++;
                }
            }
        }

        public void RemoveEntryPoint(EntryPoint entry) {
            if(!Active) {
                if (entry != null) {
                    EntryPoints.Remove(entry);
                    TotalEntryPoints--;
                }
            }
        }

        private void StartEntryPoints() {
            foreach(EntryPoint entry in EntryPoints) {
                Task.Run(() => entry.OnStart());
            }
        }
        #endregion
    }
}
