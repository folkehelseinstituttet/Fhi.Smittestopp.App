using System;

namespace NDB.Covid19.Utils
{
    public class EncodingUtils
    {
        public EncodingUtils()
        {
        }

        public static string ConvertByteArrayToString(byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }
    }
}