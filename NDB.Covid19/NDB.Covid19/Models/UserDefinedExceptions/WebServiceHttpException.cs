using System;
using System.Net.Http;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    public class WebServiceHttpException : Exception
    {
        public WebServiceHttpException()
        {
        }

        public WebServiceHttpException(string message) : base(message)
        {
        }

        public WebServiceHttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public WebServiceHttpException(HttpResponseMessage response, string url)
            : base("Call to " + url + " failed: " + response)
        {
        }
    }
}