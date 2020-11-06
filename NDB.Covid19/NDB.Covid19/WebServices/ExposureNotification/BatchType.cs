namespace NDB.Covid19.WebServices.ExposureNotification
{
    public enum BatchType
    {
        ALL,
        NO
    }

    public static class BatchTypeExtensions
    {
        public static string ToTypeString(this BatchType type)
        {
            if (type == BatchType.ALL)
            {
                return "all";
            }
            else
            {
                return "no";
            }
        }

        public static BatchType ToBatchType(this string type)
        {
            if (type == "all")
            {
                return BatchType.ALL;
            }
            else
            {
                return BatchType.NO;
            }
        }
    }
}
