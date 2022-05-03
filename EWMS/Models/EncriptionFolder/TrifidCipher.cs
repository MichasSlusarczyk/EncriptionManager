using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class TrifidCipher : ICipher
    {
        private TrifidParams _trifidParam;
        public TrifidParams TrifidParam { get => _trifidParam; set => _trifidParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        private string[,] _tableOfSigns1;
        private string[,] _tableOfSigns2;
        private string[,] _tableOfSigns3;

        public TrifidCipher()
        {
            _cipherName = "Trifid Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The cipher was developed in 1901 by the French cryptologist Felix Delastelle, as an extension of the bifid cipher. It is a combination of a substitution cipher and a conversion cipher. The basis of the triple cipher are three tables, 3 × 3 in size, in which there are letters of the alphabet and a dot sign. In the encryption process, first each plaintext letter is replaced with a three-digit number representing the letter's coordinates. Then the cipher digits are rewritten from the table horizontally with successive lines, divided into three-digit groups and (using the same tables) converted back into letters, resulting in the cipher text.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Trifid;
            _trifidParam = new();
            InicializeTableOfSigns();
        }
        public TrifidCipher(object param)
        {
            _cipherName = "Trifid Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The cipher was developed in 1901 by the French cryptologist Felix Delastelle, as an extension of the bifid cipher. It is a combination of a substitution cipher and a conversion cipher. The basis of the triple cipher are three tables, 3 × 3 in size, in which there are letters of the alphabet and a dot sign. In the encryption process, first each plaintext letter is replaced with a three-digit number representing the letter's coordinates. Then the cipher digits are rewritten from the table horizontally with successive lines, divided into three-digit groups and (using the same tables) converted back into letters, resulting in the cipher text.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Trifid;
            _trifidParam = new();
            InicializeTableOfSigns();
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            TrifidParam = (TrifidParams)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            List<char> firstEncryption = new();
            List<char> afterReverse = new();
            List<byte> encryptedText = new();

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if ((check >= 'A' && check <= 'Z') || check == '.')
                {
                    for (int j = 0; j < _tableOfSigns1.GetLength(0); j++)
                    {
                        for (int k = 0; k < _tableOfSigns1.GetLength(1); k++)
                        {
                            string temp = ByteToStringConverter.ByteToString(input.DataModified[i]).ToString().ToUpper();
                            if (_tableOfSigns1[j, k].Contains(temp))
                            {
                                firstEncryption.Add('1');
                                firstEncryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstEncryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                            else if (_tableOfSigns2[j, k].Contains(temp))
                            {
                                firstEncryption.Add('2');
                                firstEncryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstEncryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                            else if (_tableOfSigns3[j, k].Contains(temp))
                            {
                                firstEncryption.Add('3');
                                firstEncryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstEncryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                        }
                    }
                }
            }

            char[,] reverseArray = new char[3, (firstEncryption.Count / 3)];

            int it = 0;

            for (int j = 0; j < (firstEncryption.Count / 3); j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    reverseArray[i, j] = firstEncryption[it];
                    it++;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < (firstEncryption.Count / 3); j++)
                {
                    afterReverse.Add(reverseArray[i, j]);
                }
            }

            for (int i = 0; i < afterReverse.Count; i += 3)
            {
                int numOfArray = Convert.ToInt32(afterReverse[i] - '0');
                int row = Convert.ToInt32(afterReverse[i + 1] - '0') - 1;
                int col = Convert.ToInt32(afterReverse[i + 2] - '0') - 1;

                char encryptedSign = '0';

                if (numOfArray == 1)
                {
                    encryptedSign = Convert.ToChar(_tableOfSigns1[row, col]);
                }
                else if (numOfArray == 2)
                {
                    encryptedSign = Convert.ToChar(_tableOfSigns2[row, col]);
                }
                else if (numOfArray == 3)
                {
                    encryptedSign = Convert.ToChar(_tableOfSigns3[row, col]);
                }

                encryptedText.Add(ByteToStringConverter.StringCharToByte(encryptedSign.ToString()));
            }

            it = 0;
            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if ((check >= 'A' && check <= 'Z') || check == '.')
                {
                    output.Add(encryptedText[it]);
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

            List<char> firstDecryption = new();
            List<char> afterReverse = new();
            List<byte> decryptedText = new();

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if ((check >= 'A' && check <= 'Z') || check == '.')
                {
                    for (int j = 0; j < _tableOfSigns1.GetLength(0); j++)
                    {
                        for (int k = 0; k < _tableOfSigns1.GetLength(1); k++)
                        {
                            string temp = ByteToStringConverter.ByteToString(input.DataModified[i]).ToString().ToUpper();
                            if (_tableOfSigns1[j, k].Contains(temp))
                            {
                                firstDecryption.Add('1');
                                firstDecryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstDecryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                            else if (_tableOfSigns2[j, k].Contains(temp))
                            {
                                firstDecryption.Add('2');
                                firstDecryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstDecryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                            else if (_tableOfSigns3[j, k].Contains(temp))
                            {
                                firstDecryption.Add('3');
                                firstDecryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstDecryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                        }
                    }
                }
            }

            char[,] reverseArray = new char[3, (firstDecryption.Count / 3)];

            int it = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < (firstDecryption.Count / 3); j++)
                {
                    reverseArray[i, j] = firstDecryption[it];
                    it++;
                }
            }

            for (int j = 0; j < (firstDecryption.Count / 3); j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    afterReverse.Add(reverseArray[i, j]);
                }
            }

            for (int i = 0; i < afterReverse.Count; i += 3)
            {
                int numOfArray = Convert.ToInt32(afterReverse[i] - '0');
                int row = Convert.ToInt32(afterReverse[i + 1] - '0') - 1;
                int col = Convert.ToInt32(afterReverse[i + 2] - '0') - 1;

                char encryptedSign = '0';

                if (numOfArray == 1)
                {
                    encryptedSign = Convert.ToChar(_tableOfSigns1[row, col]);
                }
                else if (numOfArray == 2)
                {
                    encryptedSign = Convert.ToChar(_tableOfSigns2[row, col]);
                }
                else if (numOfArray == 3)
                {
                    encryptedSign = Convert.ToChar(_tableOfSigns3[row, col]);
                }

                decryptedText.Add(ByteToStringConverter.StringCharToByte(encryptedSign.ToString()));
            }


            it = 0;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if ((check >= 'A' && check <= 'Z') || check == '.')
                {
                    output.Add(decryptedText[it]);
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
            _tableOfSigns1 = new string[3, 3] { { "A", "B", "C" }, { "I", "J", "K" }, { "R", "S", "T" } };
            _tableOfSigns2 = new string[3, 3] { { "D", ".", "E" }, { "L", "M", "N" }, { "U", "V", "W" } };
            _tableOfSigns3 = new string[3, 3] { { "F", "G", "H" }, { "O", "P", "Q" }, { "X", "Y", "Z" } };
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
            return string.Empty;
        }

        public void ResetParam()
        {
            _trifidParam = new();
        }
    }

    internal class TrifidParams { }
}



