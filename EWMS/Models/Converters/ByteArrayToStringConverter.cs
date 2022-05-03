
namespace EWMS.Models.Converters
{
    internal class ByteArrayToStringConverter
    {
        public static byte[] StringToByteArray(string input)
        {
            byte[] bytes = new byte[input.Length];

            int i = 0;

            foreach (char c in input)
            {
                bytes[i] = (byte)c;
                i++;
            }
            return bytes;
        }

        public static string ByteArrayToString(byte[] input)
        {
            string output = string.Empty;

            foreach (char c in input)
            {
                output += c;
            }
            return output;
        }
    }
}
