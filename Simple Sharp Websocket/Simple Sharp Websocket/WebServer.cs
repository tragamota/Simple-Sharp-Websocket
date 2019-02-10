using SimpleWebsocket.Server.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server
{
    public sealed class Webserver : IWebServer, IWebServerStatistics
    {
        #region Properties
        public bool Active { get; private set; }
        public List<EndPoint> EntryPoints { get; private set; }

        //interface properties
        public int ActiveEndpoints { get; private set; }
        public int ActiveServices { get; private set; }
        public int TotalEndpoints { get; private set; }
        public int TotalServices { get; private set; }
        #endregion

        public Webserver()
        {
            EntryPoints = new List<EndPoint>(20);
        }

        #region Methods
        public bool Start()
        {
            if (EntryPoints.Any())
            {
                Active = true;
                StartEntryPoints();
            }
            else
            {
                throw new Exception("There are no endpoints are provided");
            }
            return Active;
        }

        public void Stop()
        {
            if (Active)
            {
                foreach (EndPoint endPoint in EntryPoints)
                {
                    endPoint.OnStop();
                }
            }
        }

        public async Task<bool> StartAsync()
        {
            return await Task.Run(() => Start());
        }

        public async Task StopAsync()
        {
            await Task.Run(() => Stop());
        }

        public void AddEntryPoint(EndPoint endPoint)
        {
            if (endPoint != null)
            {
                if (!Active)
                {
                    EntryPoints.Add(endPoint);
                    TotalEndpoints++;
                }
            }
        }

        public void RemoveEntryPoint(EndPoint endPoint)
        {
            if (endPoint != null)
            {
                if (!Active)
                {
                    EntryPoints.Remove(endPoint);
                    TotalEndpoints--;
                }
            }
        }

        private async void StartEntryPoints()
        {
            ActiveEndpoints = await Task.Run(() => {
                int activeEntryPoints = 0;
                foreach (EndPoint entry in EntryPoints)
                {
                    if(entry.OnStart())
                    {
                        activeEntryPoints++;
                    }
                }
                return activeEntryPoints;
            });
        }

        public string Info()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine(Active ? "The server is currently running..." : "The server is currently not running...");
            infoBuilder.AppendLine("This webserver contains " + TotalEndpoints + " Endpoints");
            infoBuilder.AppendLine("There are at the moment " + ActiveServices + "/" + TotalServices + " running\n\n");

            foreach (EndPoint endPoint in EntryPoints)
            {

            }

            return infoBuilder.ToString();
        }

        public void PrintInfo()
        {
            Console.WriteLine(Info());
        }
        #endregion
    }
}
