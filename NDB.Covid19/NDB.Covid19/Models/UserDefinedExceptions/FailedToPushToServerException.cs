using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class FailedToPushToServerException : Exception
    {
        public FailedToPushToServerException()
        {
        }

        public FailedToPushToServerException(string message) : base(message)
        {
        }

        public FailedToPushToServerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}