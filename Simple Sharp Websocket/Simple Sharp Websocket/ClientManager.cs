using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_Sharp_Websocket
{
    internal class ClientManager
    {
        private Dictionary<SocketPipelineClient, Tuple<Task, Task>> _socketOperations;

        public ClientManager()
        {
            this._socketOperations = new Dictionary<SocketPipelineClient, Tuple<Task, Task>>();
        }

        public void ManageClient(NativeSocketClient newUnmanagedClient)
        {
            SocketPipelineClient client = new SocketPipelineClient(newUnmanagedClient);
            client.DisconnectEvent += ClientNotConnected;

            _socketOperations.Add(client, new Tuple<Task, Task>(client.FillPipeline(), client.ConsumePipeline()));
        }

        private void UnmanageClient(SocketPipelineClient oldManagedClient)
        {
            Tuple<Task, Task> socketOperation = _socketOperations[oldManagedClient];

            socketOperation.Item1.Wait();
            socketOperation.Item2.Wait();

            _socketOperations.Remove(oldManagedClient);
        }

        private void ClientNotConnected(object disconnectedClient, EventArgs args)
        {
            if (disconnectedClient != null)
            {
                UnmanageClient(disconnectedClient as SocketPipelineClient);
            }
        }
    }
}