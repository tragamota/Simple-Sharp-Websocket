using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsocket
{
    internal class HttpRequestReader
    {
        private Stream clientStream;
        private BinaryReader reader;

        public HttpRequestReader(Stream clientStream)
        {
            this.clientStream = clientStream;
            reader = new BinaryReader(clientStream, Encoding.UTF8, false);
        }

        public HttpRequest ReadHttpRequest()
        {
            HttpRequest request = CreateRequest(reader.ReadHttpLine());

            if (request != null)
            {
                string currentLine;
                while ((currentLine = reader.ReadHttpLine()) != string.Empty)
                {
                    ProcessLine(ref currentLine, ref request);
                }

                if (request.ContainsHeader("Content-Length"))
                {
                    int bodySize = (int)request.RetrieveHeader("Content-Length");
                    request.AddBody(reader.ReadBytes(bodySize));
                }
            }

            return request;
        }

        public Task<HttpRequest> ReadHttpRequestAsync()
        {
            return Task.Run(() =>
            {
                return ReadHttpRequest();
            });
        }

        private HttpRequest CreateRequest(string methodLine)
        {
            HttpRequest returnRequestType = null;
            string[] httpMethodPieces = methodLine.Split(' ');

            if (httpMethodPieces.Length == 3)
            {
                if (httpMethodPieces[2] == "HTTP/1.1")
                {
                    if (Enum.TryParse(httpMethodPieces[0], out HttpMethod requestMethod))
                    {
                        returnRequestType = new HttpRequest(requestMethod, httpMethodPieces[1]);
                    }
                }
            }

            return returnRequestType;
        }

        private void ProcessLine(ref string httpHeader, ref HttpRequest request)
        {
            if (request != null)
            {
                string[] splitHeader = httpHeader.Split(':');
                request.AddHeader(splitHeader[0], splitHeader[1].TrimStart());
            }
        }

        private void ReadRequestBody(ref int totalBytes, ref HttpRequest request) 
        {
            int readBytes = 0;

            while(readBytes < totalBytes)
            {
                request.AddBody(reader.ReadBytes(totalBytes - readBytes));
            }
        }
    }

    public static class BinaryHttpExtension
    {
        public static string ReadHttpLine(this BinaryReader reader)
        {
            bool line = false;
            StringBuilder lineBuilder = new StringBuilder();

            char readChar;
            while(!line)
            {
                readChar = reader.ReadChar();

                if (readChar == '\r')
                {
                    reader.ReadChar();
                    line = true;
                }
                else if(readChar == '\0')
                {
                    line = true;
                }
                else
                {
                    lineBuilder.Append(readChar);
                }
            }

            return lineBuilder.ToString();
        }
    }
}