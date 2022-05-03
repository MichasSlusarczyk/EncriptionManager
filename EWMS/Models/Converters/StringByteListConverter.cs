using System;
using System.Collections.Generic;

namespace EWMS.Models.Converters
{
    internal class StringByteListConverter
    {
        public static List<byte> StringToByteList(string text)
        {
            List<byte> bytesList = new();

            foreach (char c in text)
            {
                bytesList.Add((byte)c);
            }

            return bytesList;
        }

        public static string ByteListToString(List<byte> bytes)
        {
            string text = string.Empty;

            foreach (byte b in bytes)
            {
                text += Convert.ToChar(b);
            }

            return text;
        }
    }
}
