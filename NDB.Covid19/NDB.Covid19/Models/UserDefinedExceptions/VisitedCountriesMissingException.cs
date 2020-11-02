using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class VisitedCountriesMissingException : Exception
    {
        public VisitedCountriesMissingException()
        {
        }

        public VisitedCountriesMissingException(string message) : base(message)
        {
        }

        public VisitedCountriesMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}