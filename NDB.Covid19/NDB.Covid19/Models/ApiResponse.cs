using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using NDB.Covid19.Configuration;

namespace NDB.Covid19.Models
{
    public class ApiResponse
    {
        public HttpMethod HttpMethod { get; set; }
        public string Endpoint { get; set; }
        public string ResponseText { get; set; }

        public int StatusCode { get; set; }
        public Exception Exception { get; set; }
        public HttpHeaders Headers { get; set; }
        public bool IsSuccessfull => HasSuccessfullStatusCode && Exception == null;
        public bool HasSuccessfullStatusCode => (StatusCode == (int)HttpStatusCode.OK || StatusCode == (int)HttpStatusCode.Created ||
            StatusCode == (int)HttpStatusCode.NoContent);

        public string ErrorLogMessage
        {
            get
            {
                string endpoint = Endpoint;
                if (endpoint.EndsWith(".zip"))
                {
                    //Remove the last part e.g. "2020-09-03:1.no.zip", so it can be summarized in logs with a generic message
                    int index = endpoint.IndexOf(endpoint.Split('/').Last());
                    if (index > 1)
                    {
                        endpoint = endpoint.Remove(index - 1);
                    }
                }

                string message = $"API {HttpMethod} /{endpoint} failed";
                if (!HasSuccessfullStatusCode && StatusCode != 0)
                {
                    message += $" with HttpStatusCode {StatusCode}";
                }
                else if (Exception != null)
                {
                    message += $" with {Exception.GetType().Name}";
                }
                return message;
            }
        }

        public ApiResponse(string url, HttpMethod method)
        {
            HttpMethod = method;
            try
            {
                Endpoint = url.Split(new string[] { Conf.BASE_URL }, StringSplitOptions.None).Last();
            }
            catch { }
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponse(string url, HttpMethod method) : base(url, method)
        {
            Data = default(T);
        }
    }
}
