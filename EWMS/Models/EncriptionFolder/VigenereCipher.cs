 using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EWMS.Models.EncryptionFolder
{
    internal class VigenereCipher : ICipher
    {
        private VigenereParams _vigenereParam;
        public VigenereParams VigenereParam { get => _vigenereParam; set => _vigenereParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        private readonly char[,] _tableOfSigns;
        public VigenereCipher()
        {
            _cipherName = "Vigenere Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "Keyword - a-Z text.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The author of the Vigenère cipher is the Italian Giovan Battista Bellaso, who described it in 1553 in his book \"La cifra del.Sig.Giovan Battista Bellaso\". The name of this cipher comes from the erroneous and widely accepted belief in the 19th century that its creator was the French diplomat and alchemist Blaise de Vigenère, who lived in the 16th century. The Vigenère cipher is a polyalphabetic substitution cipher. So it consists in using variously defined substitutions for subsequent letters of the text.Contrary to some polyalphabetic ciphers, the pages do not have to remember and exchange all defined alphabetic transformations among themselves.The solution used reduces the amount of information that needs to be remembered to just one secret word(or sentence). For encryption and decryption, an array is used that contains the letters of the alphabet in the original order in the first row, and then in each successive row the letters of the alphabet shifted one position to the left.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Vigenere;
            _vigenereParam = new();
            _tableOfSigns = new char[26, 26];
            InicializeTableOfSigns();
        }

        public VigenereCipher(object param)
        {
            _cipherName = "Vigenere Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "Keyword - a-Z text.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The author of the Vigenère cipher is the Italian Giovan Battista Bellaso, who described it in 1553 in his book \"La cifra del.Sig.Giovan Battista Bellaso\". The name of this cipher comes from the erroneous and widely accepted belief in the 19th century that its creator was the French diplomat and alchemist Blaise de Vigenère, who lived in the 16th century. The Vigenère cipher is a polyalphabetic substitution cipher. So it consists in using variously defined substitutions for subsequent letters of the text.Contrary to some polyalphabetic ciphers, the pages do not have to remember and exchange all defined alphabetic transformations among themselves.The solution used reduces the amount of information that needs to be remembered to just one secret word(or sentence). For encryption and decryption, an array is used that contains the letters of the alphabet in the original order in the first row, and then in each successive row the letters of the alphabet shifted one position to the left.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Vigenere;
            _vigenereParam = new();
            _tableOfSigns = new char[26, 26];
            InicializeTableOfSigns();
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            VigenereParam = (VigenereParams)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            string tempMessage = StringByteListConverter.ByteListToString(input.DataModified).ToUpper();

            string tempKeyWord = VigenereParam.Keyword;
            tempKeyWord = string.Concat(tempKeyWord.Where(c => !char.IsWhiteSpace(c)));
            tempKeyWord = tempKeyWord.ToUpper();

            int messageLength = 0;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    messageLength++;
                }
            }

            if (messageLength < tempKeyWord.Length)
            {
                tempKeyWord = tempKeyWord.Substring(0, messageLength);
            }
            else if (messageLength > tempKeyWord.Length)
            {
                tempKeyWord = AlignText(tempKeyWord, messageLength);
            }


            int it = 0;
            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    int indexX = ReturnIndex(tempMessage[i]);
                    int indexY = ReturnIndex(tempKeyWord[it]);
                    output.Add((byte)_tableOfSigns[indexX, indexY]);
                    it++;
                }
                else
                {
                    output.Add(input.DataModified[i]);
                }
            }

            return output;
        }

        public List<byte> Decrypt(Data input)
        {
            List<byte> output = new();

            string tempMessage = StringByteListConverter.ByteListToString(input.DataModified).ToUpper();

            string tempKeyWord = VigenereParam.Keyword;
            tempKeyWord = string.Concat(tempKeyWord.Where(c => !char.IsWhiteSpace(c)));
            tempKeyWord = tempKeyWord.ToUpper();

            int messageLength = 0;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    messageLength++;
                }
            }

            if (messageLength < tempKeyWord.Length)
            {
                tempKeyWord = tempKeyWord.Substring(0, messageLength);
            }
            else if (messageLength > tempKeyWord.Length)
            {
                tempKeyWord = AlignText(tempKeyWord, messageLength);
            }

            int it = 0;
            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    int indexX = ReturnIndex(tempKeyWord[it]);
                    int indexY = 0;
                    for (int j = 0; j < _tableOfSigns.GetLength(0); j++)
                    {
                        if (tempMessage[i] == _tableOfSigns[j, indexX])
                        {
                            indexY = j;
                        }
                    }
                    output.Add((byte)_tableOfSigns[0, indexY]);
                    it++;
                }
                else
                {
                    output.Add(input.DataModified[i]);
                }
            }

            return output;
        }

        private void InicializeTableOfSigns()
        {
            for (int i = 0; i < 26; i++)
            {
                _tableOfSigns[0, i] = (char)(65 + i);
            }


            for (int r = 1; r < 26; r++)
            {
                for (int i = 0; i < 26; i++)
                {
                    _tableOfSigns[r, i] = _tableOfSigns[0, (i + r) % 26];
                }
            }
        }

        private static string AlignText(string text, int length)
        {
            int originalTextLength = text.Length;

            int it = 0;
            while (text.Length != length)
            {
                text += text[it];
                it++;

                if (it == originalTextLength)
                {
                    it = 0;
                }
            }

            return text;
        }

        private int ReturnIndex(char sign)
        {
            int ret = 0;

            for (int i = 0; i < _tableOfSigns.GetLength(0); i++)
            {
                if (_tableOfSigns[0, i] == sign)
                {
                    ret = i;
                }
            }

            return ret;
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
            int messageLength = 0;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    messageLength++;
                }
            }

            string message = string.Empty;

            if (_vigenereParam.Keyword.Length <= 0 || _vigenereParam.Keyword.Length > messageLength)
            {
                message += "The keyword length should be in the range from 1 to " + messageLength.ToString() + "!";
            }

            return message;
        }

        public void ResetParam()
        {
            _vigenereParam = new();
        }
    }

    internal class VigenereParams
    {
        private string _keyword;

        public VigenereParams()
        {
            _keyword = string.Empty;
        }
        public string Keyword { get => _keyword; set => _keyword = value; }
    }
}



