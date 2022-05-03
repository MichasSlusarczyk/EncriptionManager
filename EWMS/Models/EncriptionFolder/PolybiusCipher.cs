using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class PolybiusCipher : ICipher
    {
        private PolybiusParams _polybiusParam;
        public PolybiusParams PolybiusParam { get => _polybiusParam; set => _polybiusParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        private readonly string[,] tableOfSigns;

        public PolybiusCipher()
        {
            _cipherName = "Polybius Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted and only number separated with space and mathing the algoritm are decrypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with deleted characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The Greek historian Polybius invented a simple cipher named after him. The key to the cipher is the table with the alphabet in it. The encrypted information is represented by a series of digits representing the position of a given letter in the table: the first digit is the row number, the second digit - the column.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Polybius;
            _polybiusParam = new();
            tableOfSigns = new string[5, 5];
            InicializeTableOfSigns();
        }
        public PolybiusCipher(object param)
        {
            _cipherName = "Polybius Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted and only number separated with space and mathing the algoritm are decrypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with deleted characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The Greek historian Polybius invented a simple cipher named after him. The key to the cipher is the table with the alphabet in it. The encrypted information is represented by a series of digits representing the position of a given letter in the table: the first digit is the row number, the second digit - the column.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Polybius;
            _polybiusParam = new();
            tableOfSigns = new string[5, 5];
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            PolybiusParam = (PolybiusParams)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    for (int j = 0; j < tableOfSigns.GetLength(0); j++)
                    {
                        for (int k = 0; k < tableOfSigns.GetLength(1); k++)
                        {
                            string temp = ByteToStringConverter.ByteToString(input.DataModified[i]).ToString().ToUpper();
                            if (tableOfSigns[j, k].Contains(temp))
                            {
                                output.Add(ByteToStringConverter.StringCharToByte((j + 1).ToString()));
                                output.Add(ByteToStringConverter.StringCharToByte((k + 1).ToString()));
                                output.Add(ByteToStringConverter.StringCharToByte(" "));
                            }
                        }
                    }
                }
            }

            if (output.Count != 0)
            {
                output.RemoveAt(output.Count - 1);
            }

            return output;
        }

        public List<byte> Decrypt(Data input)
        {
            List<byte> output = new();
            List<byte> tempMessageEncrypted = new();

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

            for (int i = 0; i < tempMessageEncrypted.Count; i++)
            {
                if (tempMessageEncrypted[i] > 9 && tempMessageEncrypted[i] < 100)
                {
                    tempString = tempMessageEncrypted[i].ToString();
                    int indexX = tempString[0] - '0' - 1;
                    int indexY = tempString[1] - '0' - 1;
                    if (indexX >= 0 && indexX <= 4 && indexY >= 0 && indexY <= 4)
                    {
                        output.Add(ByteToStringConverter.StringCharToByte(tableOfSigns[indexX, indexY].Substring(0, 1)));
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
                    if (check >= '1' && check <= '5')
                    {
                        correctNumbers += check;
                    }
                }

                if (correctNumbers.Length != tempString.Length)
                {
                    correctNumbers = string.Empty;
                }

                if (correctNumbers != string.Empty)
                {
                    byte tempByte = ByteToStringConverter.StringNumberToByte(tempString);
                    if (tempByte >= 11 && tempByte <= 55)
                    {
                        tempMessageEncrypted.Add(ByteToStringConverter.StringNumberToByte(tempString));
                    }
                }
            }

            return tempMessageEncrypted;
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

                    tableOfSigns[i, j] = signs;
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
            _polybiusParam = new();
        }
    }

    internal class PolybiusParams { }
}



