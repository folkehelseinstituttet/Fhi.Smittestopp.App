using System;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Models.Logging
{
    public class LogExceptionDetails
    {
        readonly int _maxLengthOfStacktrace = 1000; //number of characters

        public string ExceptionType { get; private set; }
        public string ExceptionMessage { get; private set; }
        public string ExceptionStackTrace { get; private set; }
        public string InnerExceptionType { get; private set; }
        public string InnerExceptionMessage { get; private set; }
        public string InnerExceptionStackTrace { get; private set; }

        public LogExceptionDetails(Exception e)
        {
            if (e == null)
            {
                return;
            }

            ExceptionType = e.GetType().Name;
            ExceptionMessage = Anonymizer.RedactText(e.Message);
            ExceptionStackTrace = Anonymizer.RedactText(ShortenedText(e.StackTrace));

            if (e.InnerException != null)
            {
                InnerExceptionType = e.InnerException.GetType().Name;
                InnerExceptionMessage = Anonymizer.RedactText(e.InnerException.Message);
                InnerExceptionStackTrace = Anonymizer.RedactText(ShortenedText(e.InnerException.StackTrace));
            }
        }

        string ShortenedText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= _maxLengthOfStacktrace)
            {
                return text;
            }
            return text.Substring(0, _maxLengthOfStacktrace);
        }
    }
}
