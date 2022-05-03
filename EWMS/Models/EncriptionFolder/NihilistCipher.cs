using EWMS.Models.Containers;
using EWMS.Models.Converters;
using EWMS.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EWMS.Models.EncryptionFolder
{
    internal class NihilistCipher : ICipher
    {
        //Parametry szyfru
        private NihilstParams _nihilistParam;
        public NihilstParams NihilistParam { get => _nihilistParam; set => _nihilistParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        private List<List<char>> _tableOfSigns;

        //kostruktor
        public NihilistCipher()
        {
            _cipherName = "Nihilist Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted and only number separated with space and mathing the algoritm are decrypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "First keyword - text with letters in the range a-Z with a minimum length of 0 and a maximum length of message length.\n" +
                "Second keyword - text with letters in the range a-Z with a minimum length of 0 and a maximum length of message length.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with deleted characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "First used in the 1880s in Russia by Nihilist organizations. The Nihilist cipher algorithm uses a matrix called Polybius square in its operation. This matrix measures 5x5 and is filled with all the letters of the alphabet. The order of letters in the matrix depends on a secret keyword that is shared between the communicating parties. In order to determine the order of letters, omit the repeated letters in the keyword, and then enter the remaining letters into the table. The rest of the fields in the matrix are filled with the remaining letters from the alphabet that were not included in the keyword. Each letter of the secret key and each letter of the message is converted into a two-digit number indicated by a line number and a column number. Encryption consists in adding successive numbers made of the letters of the plaintext and the secret key.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Nihilist;
            _nihilistParam = new();
            _tableOfSigns = new();
        }

        //kostruktor
        public NihilistCipher(object param)
        {
            _cipherName = "Nihilist Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted and only number separated with space and mathing the algoritm are decrypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "First keyword - text with letters in the range a-Z with a minimum length of 0 and a maximum length of message length.\n" +
                "Second keyword - text with letters in the range a-Z with a minimum length of 0 and a maximum length of message length.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with deleted characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "First used in the 1880s in Russia by Nihilist organizations. The Nihilist cipher algorithm uses a matrix called Polybius square in its operation. This matrix measures 5x5 and is filled with all the letters of the alphabet. The order of letters in the matrix depends on a secret keyword that is shared between the communicating parties. In order to determine the order of letters, omit the repeated letters in the keyword, and then enter the remaining letters into the table. The rest of the fields in the matrix are filled with the remaining letters from the alphabet that were not included in the keyword. Each letter of the secret key and each letter of the message is converted into a two-digit number indicated by a line number and a column number. Encryption consists in adding successive numbers made of the letters of the plaintext and the secret key.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Nihilist;
            _tableOfSigns = new();
            InicializeTableOfSigns();
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            NihilistParam = (NihilstParams)param;
            InicializeTableOfSigns();
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            string tempMessage = string.Empty;

            InicializeTableOfSigns();

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    tempMessage += check;
                }
            }

            tempMessage = string.Concat(tempMessage.Where(c => !char.IsWhiteSpace(c)));
            tempMessage = tempMessage.ToUpper();

            string tempkeyWord2 = NihilistParam.Keyword2;
            tempkeyWord2 = string.Concat(tempkeyWord2.Where(c => !char.IsWhiteSpace(c)));
            tempkeyWord2 = tempkeyWord2.ToUpper();

            List<byte> messageEncrypted = new();
            List<byte> keyWord2Encrited = new();

            messageEncrypted = ReturnEncryptedText(tempMessage);
            keyWord2Encrited = ReturnEncryptedText(tempkeyWord2);

            if (keyWord2Encrited.Count > messageEncrypted.Count)
            {
                keyWord2Encrited.RemoveRange(messageEncrypted.Count, messageEncrypted.Count - messageEncrypted.Count);
            }
            else if (keyWord2Encrited.Count < messageEncrypted.Count)
            {
                keyWord2Encrited = Align(keyWord2Encrited, messageEncrypted.Count);
            }

            for (int i = 0; i < messageEncrypted.Count; i++)
            {
                byte tempByte = (byte)(messageEncrypted[i] + keyWord2Encrited[i]);
                string temp = tempByte.ToString();
                foreach (char c in temp)
                {
                    output.Add((byte)c);
                }
                output.Add(ByteToStringConverter.StringCharToByte(" "));
            }

            return output;
        }

        public List<byte> Decrypt(Data input)
        {
            List<byte> output = new();

            List<byte> tempMessageEncrypted = new();
            List<byte> messageEncrypted = new();

            InicializeTableOfSigns();

            string tempkeyWord2 = NihilistParam.Keyword2;
            tempkeyWord2 = string.Concat(tempkeyWord2.Where(c => !char.IsWhiteSpace(c)));
            tempkeyWord2 = tempkeyWord2.ToUpper();

            List<byte> keyWord2Encrited = new();

            string tempString = string.Empty;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                if (input.DataModified[i] != 32)
                {
                    tempString += ByteToStringConverter.ByteToString(input.DataModified[i]);
                }
                else
                {
                    tempMessageEncrypted.AddRange(ValidateSubstring(tempString));
                    tempString = string.Empty;
                }
            }

            tempMessageEncrypted.AddRange(ValidateSubstring(tempString));

            keyWord2Encrited = ReturnEncryptedText(tempkeyWord2);

            if (keyWord2Encrited.Count > tempMessageEncrypted.Count)
            {
                keyWord2Encrited.RemoveRange(tempMessageEncrypted.Count, keyWord2Encrited.Count - tempMessageEncrypted.Count);
            }
            else if (keyWord2Encrited.Count < tempMessageEncrypted.Count)
            {
                keyWord2Encrited = Align(keyWord2Encrited, tempMessageEncrypted.Count);
            }

            for (int i = 0; i < tempMessageEncrypted.Count; i++)
            {

                messageEncrypted.Add((byte)(tempMessageEncrypted[i] - keyWord2Encrited[i]));

            }

            for (int i = 0; i < messageEncrypted.Count; i++)
            {
                if (messageEncrypted[i] > 9 && messageEncrypted[i] < 100)
                {
                    tempString = messageEncrypted[i].ToString();
                    int indexX = tempString[0] - '0' - 1;
                    int indexY = tempString[1] - '0' - 1;
                    if (indexX >= 0 && indexX <= 4 && indexY >= 0 && indexY <= 4)
                    {
                        output.Add((byte)_tableOfSigns[indexX][indexY]);
                    }
                }
            }

            return output;
        }

        private static List<byte> ValidateSubstring(string tempString)
        {
            List<byte> tempMessageEncrypted = new();

            if (tempString != string.Empty && tempString != " ")
            {
                string correctNumbers = string.Empty;
                for (int j = 0; j < tempString.Length; j++)
                {
                    char check = tempString[j];
                    if (check >= '0' && check <= '9')
                    {
                        correctNumbers += check;
                    }
                }

                if (correctNumbers.Length != tempString.Length || !ValidateCommonInput.ValidateTextToNumberBool(correctNumbers))
                {
                    correctNumbers = string.Empty;
                }

                if (correctNumbers != string.Empty)
                {
                    if (Convert.ToInt32(tempString) >= 0 && Convert.ToInt32(tempString) < 256)
                    {
                        byte tempByte = ByteToStringConverter.StringNumberToByte(tempString);
                        tempMessageEncrypted.Add(tempByte);
                    }

                }
            }

            return tempMessageEncrypted;
        }
        private void InicializeTableOfSigns()
        {
            _tableOfSigns = new();
            string keyWord1 = NihilistParam.Keyword1;
            keyWord1 = string.Concat(keyWord1.Where(c => !char.IsWhiteSpace(c)));
            keyWord1 = keyWord1.ToUpper();
            keyWord1 = keyWord1.Replace('J', 'I');
            keyWord1 = RemoveDuplicates(keyWord1);
            int keyWord1Length = keyWord1.Length;
            int restLength = 25 - keyWord1.Length;

            for (int i = 0; i < 5; i++)
            {
                _tableOfSigns.Add(new List<char>());
            }

            for (int i = 0; i < keyWord1Length; i++)
            {
                int indexRow = (int)Math.Floor((double)(i / 5));
                _tableOfSigns[indexRow].Add(keyWord1[i]);
            }

            for (int i = keyWord1Length; i < 25; i++)
            {
                int indexRow = (int)Math.Floor((double)(i / 5));
                char sign = (char)65;
                for (int j = 0; j < 26; j++)
                {
                    if (!CheckSignInTable(_tableOfSigns, (char)(sign + j)))
                    {
                        sign = (char)(sign + j);
                        j = 26;
                    }
                }

                _tableOfSigns[indexRow].Add(sign);
            }
        }

        private static string RemoveDuplicates(string input)
        {
            return new string(input.ToCharArray().Distinct().ToArray());
        }

        private static bool CheckSignInTable(List<List<char>> tableOfSigns, char sign)
        {
            if (sign == 'J')
            {
                return true;
            }

            bool isContains = false;
            for (int i = 0; i < tableOfSigns.Count; i++)
            {
                if (tableOfSigns[i].Contains(sign))
                {
                    isContains = true;
                    i = tableOfSigns.Count;
                }
            }

            return isContains;
        }

        private List<byte> ReturnEncryptedText(string text)
        {
            List<byte> textEncrited = new();

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < _tableOfSigns.Count; j++)
                {
                    char b = text[i];

                    if (b == 'J')
                    {
                        b = 'I';
                    }

                    if (_tableOfSigns[j].Contains(b))
                    {
                        int indexX = j;
                        int indexY = _tableOfSigns[j].IndexOf(b);
                        string tempPos = "";
                        tempPos += (indexX + 1).ToString();
                        tempPos += (indexY + 1).ToString();
                        textEncrited.Add(ByteToStringConverter.StringNumberToByte(tempPos));
                        j = _tableOfSigns.Count;
                    }
                }
            }

            return textEncrited;
        }

        private static List<byte> Align(List<byte> text, int length)
        {
            int originalTextLength = text.Count;

            int it = 0;
            while (text.Count != length)
            {
                text.Add(text[it]);
                it++;

                if (it == originalTextLength)
                {
                    it = 0;
                }
            }

            return text;
        }

        public string ReturnCipherName()
        {
            return _cipherName;
        }

        public CipherType ReturnCipherType()
        {
            return _cipherType;
        }

        public CipherClass ReturnCipherClass()
        {
            return _cipherClass;
        }

        public string ReturnCipherDescription()
        {
            return _cipherDescription;
        }

        public string CheckparamValidation()
        {
            return string.Empty;
        }

        public string CheckparamValidation(Data input)
        {
            string message = string.Empty;

            if (input.DataModified.Count < _nihilistParam.Keyword1.Length)
            {
                message += "The first keyword length should be smaller or equal " + input.DataModified.Count.ToString() + "!\n";
            }

            if (_nihilistParam.Keyword2.Length < 1 || input.DataModified.Count < _nihilistParam.Keyword2.Length)
            {
                message += "The second keyword length should be in the range from 1 to " + input.DataModified.Count.ToString() + "!";
            }

            return message;
        }

        public void ResetParam()
        {
            _nihilistParam = new();
        }
    }

    internal class NihilstParams
    {
        private string _keyword1;
        private string _keyword2;

        public NihilstParams()
        {
            _keyword1 = string.Empty;
            _keyword2 = string.Empty;
        }
        public string Keyword1 { get => _keyword1; set => _keyword1 = value; }
        public string Keyword2 { get => _keyword2; set => _keyword2 = value; }
    }
}



