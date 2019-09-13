using System;
using System.Net.Sockets;

namespace Simple_Sharp_Websocket
{
    public class OnNewClientEventArgs : EventArgs
    {
        public SocketClient NewClient { get; set; }
    }
}