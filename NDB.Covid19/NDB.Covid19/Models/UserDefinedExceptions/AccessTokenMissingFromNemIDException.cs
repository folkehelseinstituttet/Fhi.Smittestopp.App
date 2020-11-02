using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class AccessTokenMissingFromNemIDException : Exception
    {
        public AccessTokenMissingFromNemIDException()
        {
        }

        public AccessTokenMissingFromNemIDException(string message) : base(message)
        {
        }

        public AccessTokenMissingFromNemIDException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}