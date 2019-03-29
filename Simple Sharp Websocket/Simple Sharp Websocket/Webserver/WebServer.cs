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
        public IReadOnlyList<EndPoint> EndPoints { get { return _endPoints; } }

        //interface properties
        public int ActiveEndpoints { get; private set; }
        public int ActiveServices { get; private set; }
        public int TotalEndpoints { get; private set; }
        public int TotalServices { get; private set; }

        private List<EndPoint> _endPoints;
        #endregion

        public Webserver()
        {
            _endPoints = new List<EndPoint>(10);
        }

        #region Methods
        public bool Start()
        {
            if (_endPoints.Any())
            {
                Active = true;
                StartEntryPoints();
            }
            else
            {
                throw new Exception("There are no endpoints provided");
            }
            return Active;
        }

        public void Stop()
        {
            if (Active)
            {
                foreach (EndPoint endPoint in _endPoints)
                {
                    endPoint.OnStop();
                }
                Active = false;
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
                    _endPoints.Add(endPoint);
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
                    _endPoints.Remove(endPoint);
                    TotalEndpoints--;
                }
            }
        }

        private async void StartEntryPoints()
        {
            ActiveEndpoints = await Task.Run(() => {
                int activeEntryPoints = 0;
                foreach (EndPoint entry in _endPoints)
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

            foreach (EndPoint endPoint in _endPoints)
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
