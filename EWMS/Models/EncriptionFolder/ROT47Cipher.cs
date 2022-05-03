using EWMS.Models.Containers;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class ROT47Cipher : ICipher
    {
        private ROT47Params _ROT47Param;
        public ROT47Params ROT47Param { get => _ROT47Param; set => _ROT47Param = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        public ROT47Cipher()
        {
            _cipherName = "ROT47 Cipher";
            _cipherDescription =
             "INPUT:\n" +
            "Unlimited text where only characters in the decimal ASCII range 32-127 are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
            "OUTPUT:\n" +
            "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
            "SHORT DESCRIPTION:\n" +
            "Shift coding, replacing any ASCII character in the range 33-126 with a character 47 positions further up to but not more than 126 positions.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.ROT47;
            _ROT47Param = new();
        }
        public ROT47Cipher(object param)
        {
            _cipherName = "ROT47 Cipher";
            _cipherDescription =
            "INPUT:\n" +
            "Unlimited text where only characters in the decimal ASCII range 32-127 are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
            "OUTPUT:\n" +
            "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
            "SHORT DESCRIPTION:\n" +
            "Shift coding, replacing any ASCII character in the range 33-126 with a character 47 positions further up to but not more than 126 positions.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.ROT47;
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            ROT47Param = (ROT47Params)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            int localeShift = 47 % 94;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(input.DataModified[i]);
                if (check >= 33 && check <= 126)
                {
                    char tempChar = Convert.ToChar(input.DataModified[i]);

                    int newPos = tempChar + localeShift;

                    if (newPos > 126)
                    {
                        newPos -= 94;
                    }
                    else if (newPos < 33)
                    {
                        newPos = 94 + newPos;
                    }

                    output.Add((byte)Convert.ToChar(newPos));
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
            int localeShift = 47 % 94;

            localeShift = -localeShift;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(input.DataModified[i]);
                if (check >= 33 && check <= 126)
                {
                    char tempChar = Convert.ToChar(input.DataModified[i]);

                    int newPos = tempChar + localeShift;

                    if (newPos > 126)
                    {
                        newPos -= 94;
                    }
                    else if (newPos < 33)
                    {
                        newPos = 94 + newPos;
                    }

                    output.Add((byte)Convert.ToChar(newPos));
                }
                else
                {
                    output.Add(input.DataModified[i]);
                }
            }

            return output;
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
            _ROT47Param = new();
        }
    }

    internal class ROT47Params { }
}



