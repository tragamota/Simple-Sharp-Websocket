using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket.Server.Endpoints
{
    public interface IWebService
    {
        Url ServiceUrl { get; }

        void PassClient(HttpClient client, HttpRequest request);
    }

    public class HttpService : IWebService
    {
        public Url ServiceUrl { get; private set; }
        public List<HttpResource> resource;

        public HttpService()
        {
            ServiceUrl = new Url("*");
        }

        public HttpService(string host)
        {
            ServiceUrl = new Url(host);
        }

        public async void PassClient(HttpClient client, HttpRequest request)
        {
            await HandleRequest(request);
            await HandleClient(client);
        }

        private async Task HandleClient(HttpClient client) 
        {
            //check for settings if keep alive is active or request :)
            while (client.Connected)
            {
                HttpRequest request = await client.ReadHttpRequestAsync();

                if (request != null)
                {
                    SendResponse(client, await HandleRequest(request));
                }
            }

            client.Close();
        }

        private Task<HttpResponse> HandleRequest(HttpRequest request)
        {
            return Task.Run(() =>
            {
                return new HttpResponse();
            });
        }

        private void SendResponse(HttpClient client, HttpResponse response)
        {
            //client.SendHttpResponseAsync(response);
        }
    }

    public class HttpRedirectService : IWebService
    {
        public Url ServiceUrl { get; private set; }

        private string RedirectHost;
        private readonly HttpResponse RedirectResponse;

        public HttpRedirectService(string host, string redirectHost) {
            ServiceUrl = new Url(host);
            this.RedirectHost = redirectHost;

            //301 redirect HTTP
        }

       public void PassClient(HttpClient client, HttpRequest request)
       {
            string redirectResponse = "HTTP/1.1 301 Moved Permanently\r\nLocation: " + RedirectHost + request.Resource.Value + "\r\n";

            client.ClientStream.Write(Encoding.UTF8.GetBytes(redirectResponse), 0, Encoding.UTF8.GetBytes(redirectResponse).Length);
            client.Close();
       }
    }
}