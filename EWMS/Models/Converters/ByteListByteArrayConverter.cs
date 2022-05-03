using System.Collections.Generic;


namespace EWMS.Models.Converters
{
    internal class ByteListByteArrayConverter
    {
        public static List<byte> ByteArrayToByteList(byte[] bytes)
        {
            List<byte> bytesList = new();

            foreach (byte b in bytes)
            {
                bytesList.Add(b);
            }

            return bytesList;
        }

        public static byte[] ByteListToByteArray(List<byte> bytes)
        {
            byte[] bytesArray = new byte[bytes.Count];

            for (int i = 0; i < bytes.Count; i++)
            {
                bytesArray[i] = bytes[i];
            }

            return bytesArray;
        }
    }
}
