using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class AccessTokenMissingFromIDPortenException : Exception
    {
        public AccessTokenMissingFromIDPortenException()
        {
        }

        public AccessTokenMissingFromIDPortenException(string message) : base(message)
        {
        }

        public AccessTokenMissingFromIDPortenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}