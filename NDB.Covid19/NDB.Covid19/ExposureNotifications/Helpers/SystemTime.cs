using System;

namespace NDB.Covid19.ExposureNotifications.Helpers
{
    /// <summary>
    /// This is a wrapper for DateTime.Now to be able to mock it in unittests.
    /// </summary>
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;
        
        public static void SetDateTime(DateTime dateTimeNow)
        {
            Now = dateTimeNow.ToUniversalTime;
        }
        
        public static void ResetDateTime()
        {
            Now = () => DateTime.UtcNow;
        }       
    }
}