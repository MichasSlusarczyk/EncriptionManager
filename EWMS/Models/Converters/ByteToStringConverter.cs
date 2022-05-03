using System;

namespace EWMS.Models.Converters
{
    internal class ByteToStringConverter
    {
        public static string ByteToString(byte input)
        {
            return Convert.ToChar(input).ToString();
        }

        public static byte StringNumberToByte(string input)
        {

            return (byte)Convert.ToChar(Convert.ToInt32(input));
        }

        public static byte StringCharToByte(string input)
        {

            return (byte)Convert.ToChar(input);
        }
    }
}
