using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class DSOSDateMissingException : Exception
    {
        public DSOSDateMissingException()
        {
        }

        public DSOSDateMissingException(string message) : base(message)
        {
        }

        public DSOSDateMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}