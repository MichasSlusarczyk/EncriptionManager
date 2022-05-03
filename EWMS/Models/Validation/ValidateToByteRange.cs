using System.Collections.Generic;

namespace EWMS.Models.Validation
{
    internal class ValidateToByteRange
    {
        public static List<byte> ValidateTextToByteList(string input)
        {
            List<byte> output = new();

            foreach (char c in input)
            {
                if (c >= 0 && c <= 255)
                {
                    output.Add((byte)c);
                }
            }

            return output;
        }

        public static string ValidateTextToString(string input)
        {
            string output = string.Empty;

            foreach (char c in input)
            {
                if (c >= 0 && c <= 255)
                {
                    output += c;
                }
            }

            return output;
        }
    }
}
