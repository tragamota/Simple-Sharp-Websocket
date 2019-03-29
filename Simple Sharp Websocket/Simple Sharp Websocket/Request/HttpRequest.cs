using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;

namespace SimpleWebsocket
{
    public enum HttpMethod
    {
        GET, HEAD, POST, PUT, DELETE, CONNECT, OPTIONS, TRACE
    }

    public class HttpRequest : IDisposable
    {
        private readonly Dictionary<string, object> Headers;

        public HttpMethod Method { get; private set; }
        public Url Resource { get; private set; }

        public string Host { get { return (string) Headers["Host"]; } }
        public string UserAgent { get { return (string)Headers["User-Agent"]; } }
        public string Connection { get { return (string) Headers["Connection"]; } }

        public List<MimeType> Accept { get { return (List<MimeType>) Headers["Accept"]; } }
        public List<string> AcceptEncoding { get { return (List<string>)Headers["Accept-Encoding"]; } }

        public string CacheControl { get { return (string)Headers["Cache-Control"]; } }

        public MemoryStream Body { get; private set; }

        public HttpRequest(HttpMethod method, Url resource)
        {
            Headers = new Dictionary<string, object>();

            this.Method = method;
            this.Resource = resource;
            this.Body = new MemoryStream();
        }

        public bool ContainsHeader(string key)
        {
            bool containsHeader = false;

            containsHeader = Headers.ContainsKey(key);
            if(!containsHeader)
            {
                containsHeader = Headers.ContainsKey(key.ToLower());
            }

            return containsHeader;
        }

        public void AddHeader(string key, object value)
        {
            Headers.Add(key, value);
        }

        public object RetrieveHeader(string key)
        {
            return Headers[key];
        }

        public void AddBody(byte[] body)
        {
            Body.Write(body, 0, body.Length);
        }
        
        public void Dispose()
        {
            Body.Close();
            Body.Dispose();
        }
    }
}