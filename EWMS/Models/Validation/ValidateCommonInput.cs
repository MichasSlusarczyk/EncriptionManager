namespace EWMS.Models.Validation
{
    internal class ValidateCommonInput
    {
        public static string ValidateTextToPositiveNumber(string input)
        {
            string output = string.Empty;

            if (input.Length == 0)
            {
                output += '0';
            }
            else
            {
                foreach (char c in input)
                {
                    if (c >= '0' && c <= '9')
                    {
                        output += c;
                    }
                }

                while (output.Length > 1 && output.StartsWith('0'))
                {
                    output = output.Remove(0, 1);
                }

                if (output.Length == 0)
                {
                    output += '0';
                }
            }

            return output;
        }

        public static string ValidateTextToPrintableSigns(string input)
        {
            string output = string.Empty;


            foreach (char c in input)
            {
                if (c >= 32 && c <= 127)
                {
                    output += c;
                }
            }

            return output;
        }

        public static string ValidateTextToLettersOnly(string input)
        {
            string output = string.Empty;


            foreach (char c in input)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    output += c;
                }
            }

            return output;
        }

        public static string ValidateTextToNumber(string input)
        {
            string output = string.Empty;

            if (input.Length == 0)
            {
                output += '0';
            }
            else
            {
                foreach (char c in input)
                {
                    if ((c >= '0' && c <= '9') || c == '-')
                    {
                        output += c;
                    }
                }

                while (output.Length > 1 && output.StartsWith('0'))
                {
                    output = output.Remove(0, 1);
                }

                if (output.Length > 1 && output.StartsWith('-'))
                {
                    for (int i = 1; i < output.Length; i++)
                    {
                        if (output[i] == '-')
                        {
                            output = output.Remove(i, 1);
                            i--;
                        }
                    }
                }
                else if (output.Length == 1 && output.StartsWith('-'))
                {
                    output += '0';
                }

                if (output.Length > 2 && output.StartsWith("-0"))
                {

                    output = output.Remove(1, 1);
                }

                if (output.Length == 0)
                {
                    output += '0';
                }
            }

            return output;
        }

        public static bool ValidateTextToNumberBool(string input)
        {
            if (input.Length == 0)
            {
                return false;
            }
            else if (input.Length > 1 && input.StartsWith('0'))
            {
                return false;
            }


            return true;
        }

        public static string ValidateTextMaxLength(string input, int maxLength)
        {
            if (input == string.Empty || input.Length < maxLength)
            {
                return input;
            }
            else
            {
                return input.Substring(0, maxLength);
            }
        }

        public static bool ValidateTextMaxValue(string input, long maxValue)
        {
            long longInput = long.Parse(input);
            if (longInput > maxValue)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
