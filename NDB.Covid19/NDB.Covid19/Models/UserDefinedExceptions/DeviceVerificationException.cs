using System;

namespace NDB.Covid19.Models.UserDefinedExceptions
{
    [Serializable]
    public class DeviceVerificationException : Exception
    {
        public DeviceVerificationException()
        {
        }

        public DeviceVerificationException(string message) : base(message)
        {
        }

        public DeviceVerificationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}