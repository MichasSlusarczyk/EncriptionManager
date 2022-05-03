using EWMS.Models.Containers;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class ROT13Cipher : ICipher
    {
        private ROT13Params _ROT13Param;
        public ROT13Params ROT13Param { get => _ROT13Param; set => _ROT13Param = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        public ROT13Cipher()
        {
            _cipherName = "ROT13 Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "A simple shift cipher that works by replacing each Latin character with a character that follows 13 positions after it, with no case being transformed. ROT13 is an example of a Caesar cipher, developed in ancient Rome.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.ROT13;
            _ROT13Param = new();
        }

        public ROT13Cipher(object param)
        {
            _cipherName = "ROT13 Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "A simple shift cipher that works by replacing each Latin character with a character that follows 13 positions after it, with no case being transformed. ROT13 is an example of a Caesar cipher, developed in ancient Rome.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.ROT13;
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            ROT13Param = (ROT13Params)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            int localeShift = 13 % 26;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    char tempChar = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());

                    int newPos = tempChar + localeShift;

                    if (newPos > 90)
                    {
                        newPos -= 26;
                    }
                    else if (newPos < 65)
                    {
                        newPos = 26 + newPos;
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
            int localeShift = 13 % 26;

            localeShift = -localeShift;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    char tempChar = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());

                    int newPos = tempChar + localeShift;

                    if (newPos > 90)
                    {
                        newPos -= 26;
                    }
                    else if (newPos < 65)
                    {
                        newPos = 26 + newPos;
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
            _ROT13Param = new();
        }
    }

    internal class ROT13Params { }
}



