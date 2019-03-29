using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    interface IWebServerStatistics : IOnInfo
    {
        int ActiveEndpoints { get; }
        int ActiveServices { get; }
        int TotalEndpoints { get; }
        int TotalServices { get; }        
    }
}
