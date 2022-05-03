using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class BifidCipher : ICipher
    {
        private BifidParams _bifidParam;
        public BifidParams BifidParam { get => _bifidParam; set => _bifidParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        private readonly string[,] _tableOfSigns;

        //kostruktor
        public BifidCipher()
        {
            _cipherName = "Bifid Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The cipher was presented in 1895 by the French cryptologist Felix Delastelle in the French \"Revue du Génie civil\" under the name Modern Cryptography (Cryptographie Nouvelle). It is a combination of a substitution cipher and a conversion cipher. The Bifid cipher uses the Polybius checkerboard and the keyword to encrypt data. On the basis of the keyword, an alphabet is created in which the first occurrences of unique letters from the keyword appear, and then the remaining letters of the alphabet are added. The signs I and J are indistinguishable. When encrypting the data for each letter, find their positions in the Polybius board, and then write down in the column under the letter. The row number is to be above the column number. Next, successive pairs of coordinates are read by reading the numbers one by one from the row and then column markings. Each pair is then converted to a letter based on the Polybius checkerboard.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Bifid;
            _bifidParam = new();
            _tableOfSigns = new string[5, 5];
            InicializeTableOfSigns();
        }

        //kostruktor
        public BifidCipher(object param)
        {
            _cipherName = "Bifid Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The cipher was presented in 1895 by the French cryptologist Felix Delastelle in the French \"Revue du Génie civil\" under the name Modern Cryptography (Cryptographie Nouvelle). It is a combination of a substitution cipher and a conversion cipher. The Bifid cipher uses the Polybius checkerboard and the keyword to encrypt data. On the basis of the keyword, an alphabet is created in which the first occurrences of unique letters from the keyword appear, and then the remaining letters of the alphabet are added. The signs I and J are indistinguishable. When encrypting the data for each letter, find their positions in the Polybius board, and then write down in the column under the letter. The row number is to be above the column number. Next, successive pairs of coordinates are read by reading the numbers one by one from the row and then column markings. Each pair is then converted to a letter based on the Polybius checkerboard.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Bifid;
            _bifidParam = new();
            _tableOfSigns = new string[5, 5];
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            BifidParam = (BifidParams)param;
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
                if (check >= 'A' && check <= 'Z')
                {
                    for (int j = 0; j < _tableOfSigns.GetLength(0); j++)
                    {
                        for (int k = 0; k < _tableOfSigns.GetLength(1); k++)
                        {
                            string temp = ByteToStringConverter.ByteToString(input.DataModified[i]).ToString().ToUpper();
                            if (_tableOfSigns[j, k].Contains(temp))
                            {
                                firstEncryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstEncryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                        }
                    }
                }
            }

            char[,] reverseArray = new char[2, (firstEncryption.Count / 2)];

            int it = 0;

            for (int j = 0; j < (firstEncryption.Count / 2); j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    reverseArray[i, j] = firstEncryption[it];
                    it++;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < (firstEncryption.Count / 2); j++)
                {
                    afterReverse.Add(reverseArray[i, j]);
                }
            }

            for (int i = 0; i < afterReverse.Count; i += 2)
            {
                int row = Convert.ToInt32(afterReverse[i] - '0') - 1;
                int col = Convert.ToInt32(afterReverse[i + 1] - '0') - 1;

                char encryptedSign = Convert.ToChar(_tableOfSigns[row, col].Substring(0,1));

                encryptedText.Add(ByteToStringConverter.StringCharToByte(encryptedSign.ToString()));
            }

            it = 0;
            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
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
                if (check >= 'A' && check <= 'Z')
                {
                    for (int j = 0; j < _tableOfSigns.GetLength(0); j++)
                    {
                        for (int k = 0; k < _tableOfSigns.GetLength(1); k++)
                        {
                            string temp = ByteToStringConverter.ByteToString(input.DataModified[i]).ToString().ToUpper();
                            if (_tableOfSigns[j, k].Contains(temp))
                            {
                                firstDecryption.Add(Convert.ToChar((j + 1).ToString()));
                                firstDecryption.Add(Convert.ToChar((k + 1).ToString()));
                            }
                        }
                    }
                }
            }

            char[,] reverseArray = new char[2, (firstDecryption.Count / 2)];

            int it = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < (firstDecryption.Count / 2); j++)
                {
                    reverseArray[i, j] = firstDecryption[it];
                    it++;
                }
            }

            for (int j = 0; j < (firstDecryption.Count / 2); j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    afterReverse.Add(reverseArray[i, j]);
                }
            }

            for (int i = 0; i < afterReverse.Count; i += 2)
            {
                int row = Convert.ToInt32(afterReverse[i] - '0') - 1;
                int col = Convert.ToInt32(afterReverse[i + 1] - '0') - 1;

                string temp = _tableOfSigns[row, col].Substring(0, 1);
                char encryptedSign = Convert.ToChar(temp);

                decryptedText.Add(ByteToStringConverter.StringCharToByte(encryptedSign.ToString()));
            }


            it = 0;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
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
            int offset = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    string signs = ((char)(65 + i * 5 + j + offset)).ToString();
                    if (signs == "I")
                    {
                        signs = "IJ";
                        offset = 1;
                    }

                    _tableOfSigns[i, j] = signs;
                }
            }
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
            _bifidParam = new();
        }
    }

    internal class BifidParams { }
}



