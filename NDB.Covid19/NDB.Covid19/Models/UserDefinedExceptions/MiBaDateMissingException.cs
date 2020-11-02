using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class MiBaDateMissingException: Exception
    {
        public MiBaDateMissingException()
        {
        }

        public MiBaDateMissingException(string message) : base(message)
        {
        }

        public MiBaDateMissingException (string message, Exception innerException)
            : base (message, innerException)
        {
        }    
    }
}