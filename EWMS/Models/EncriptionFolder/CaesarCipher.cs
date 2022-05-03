using EWMS.Models.Containers;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class CaesarCipher : ICipher
    {
        //Parametry szyfru
        private CaesarParams _caesarParam;
        public CaesarParams CaesarParam { get => _caesarParam; set => _caesarParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        public CaesarCipher()
        {
            _cipherName = "Caesar Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "Shift - number from range -26 to 26.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "It is a type of substitution cipher in which each letter of plaintext (unencrypted) is replaced with a different letter (monoalphabetic cipher), separated from it by a fixed number of alphabetical positions, and the direction of replacement must be respected. It does not distinguish between uppercase and lowercase letters. The name of the cipher comes from Julius Caesar, who probably used this technique to communicate with his friends.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Caesar;
            _caesarParam = new();
        }

        public CaesarCipher(object param)
        {
            _cipherName = "Caesar Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "Shift - number from range -26 to 26.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption/decyption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "It is a type of substitution cipher in which each letter of plaintext (unencrypted) is replaced with a different letter (monoalphabetic cipher), separated from it by a fixed number of alphabetical positions, and the direction of replacement must be respected. It does not distinguish between uppercase and lowercase letters. The name of the cipher comes from Julius Caesar, who probably used this technique to communicate with his friends.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Caesar;
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            CaesarParam = (CaesarParams)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            int localeShift = Math.Abs(_caesarParam.Shift) % 26;

            if (_caesarParam.Shift < 0)
            {
                localeShift = -localeShift;
            }

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
            int localeShift = Math.Abs(_caesarParam.Shift) % 26;

            if (_caesarParam.Shift < 0)
            {
                localeShift = -localeShift;
            }

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
            string message = string.Empty;

            if (_caesarParam.Shift < -26 || _caesarParam.Shift > 26)
            {
                message += "The shift should be in the range from -26 to 26!";
            }

            return message;
        }

        public string CheckparamValidation(Data input)
        {
            return string.Empty;
        }

        public void ResetParam()
        {
            _caesarParam = new();
        }
    }

    internal class CaesarParams
    {
        private int _shift;

        public CaesarParams()
        {
            _shift = 0;
        }

        public int Shift { get => _shift; set => _shift = value; }

    }
}



