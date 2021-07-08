using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class FailedToFetchConfigurationException : Exception
    {
        public FailedToFetchConfigurationException()
        {
        }

        public FailedToFetchConfigurationException(string message) : base(message)
        {
        }

        public FailedToFetchConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}