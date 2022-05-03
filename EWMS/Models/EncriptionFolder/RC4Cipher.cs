using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class RC4Cipher : ICipher
    {
        private RC4Params _RC4Param;
        public RC4Params RC4Param { get => _RC4Param; set => _RC4Param = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        private byte[] T;

        public RC4Cipher()
        {
            _cipherName = "RC4 Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Any file format during encryption and dedicated file format during decryption. The maximum size of the input file is 100kB.\n\n" +
                "PARAMETERS:\n" +
                "Keyword - text with letters in the range a-Z with a minimum length of 1 and a maximum length of 256.\n\n" +
                "OUTPUT:\n" +
                "A dedicated file format during encryption and any file format during decryption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "RC4 was developed by Ron Rivest of RSA Security in 1987. Officially, the name is an acronym for \"Rivest Cipher 4\". RC4 is a stream cipher. It works by creating long sequences of key bytes which are then added to the data bytes. Encryption with RC4 consists in XOR summation of successive bytes of the message with the successive bytes of the key stream. The entire RC4 algorithm is focused on creating consecutive bytes of the stream. The key byte stream is obtained from a one-dimensional array called the T table.";
            _cipherClass = CipherClass.Stream;
            _cipherType = CipherType.RC4;
            _RC4Param = new();
        }

        public RC4Cipher(object param)
        {
            _cipherName = "RC4 Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Any file format during encryption and dedicated file format during decryption. The maximum size of the input file is 100kB.\n\n" +
                "PARAMETERS:\n" +
                "Keyword - text with letters in the range a-Z with a minimum length of 1 and a maximum length of 256.\n\n" +
                "OUTPUT:\n" +
                "A dedicated file format during encryption and any file format during decryption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "RC4 was developed by Ron Rivest of RSA Security in 1987. Officially, the name is an acronym for \"Rivest Cipher 4\". RC4 is a stream cipher. It works by creating long sequences of key bytes which are then added to the data bytes. Encryption with RC4 consists in XOR summation of successive bytes of the message with the successive bytes of the key stream. The entire RC4 algorithm is focused on creating consecutive bytes of the stream. The key byte stream is obtained from a one-dimensional array called the T table.";
            _cipherClass = CipherClass.Stream;
            _cipherType = CipherType.RC4;
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            RC4Param = (RC4Params)param;
        }

        public List<byte> Encrypt(Data input)
        {
            return Encryption(input);
        }

        public List<byte> Decrypt(Data input)
        {
            return Encryption(input);
        }

        private List<byte> Encryption(Data input)
        {
            List<byte> output = new();

            InitializeT();

            List<byte> realPassword = new();

            int tempIndex1 = 0;
            int tempIndex2 = 0;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                tempIndex1 = (tempIndex1 + 1) % 256;
                tempIndex2 = (tempIndex2 + T[tempIndex1]) % 256;

                byte buffer = T[tempIndex1];
                T[tempIndex1] = T[tempIndex2];
                T[tempIndex2] = buffer;

                realPassword.Add(T[(T[tempIndex1] + T[tempIndex2]) % 256]);

                output.Add((byte)(input.DataModified[i] ^ realPassword[i]));
            }

            return output;
        }

        private void InitializeT()
        {
            T = new byte[256];

            for (int i = 0; i < 255; i++)
            {
                T[i] = Convert.ToByte(i);
            }

            byte[] bytePassword = ByteArrayToStringConverter.StringToByteArray(_RC4Param.Keyword);

            int bytePasswordLength = bytePassword.Length;

            int tempIndex = 0;

            for (int i = 0; i < 255; i++)
            {
                tempIndex = (tempIndex + T[i] + bytePassword[i % bytePasswordLength]) % 256;

                byte buffer = T[i];
                T[i] = T[tempIndex];
                T[tempIndex] = buffer;
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
            string message = string.Empty;

            if (_RC4Param.Keyword.Length < 1 || _RC4Param.Keyword.Length > 256)
            {
                message += "The key length should be in the range from 1 to 256!";
            }

            return message;
        }

        public string CheckparamValidation(Data input)
        {
            return string.Empty;
        }

        public void ResetParam()
        {
            _RC4Param = new();
        }
    }

    internal class RC4Params
    {
        private string _keyword;

        public RC4Params()
        {
            _keyword = string.Empty;
        }
        public string Keyword { get => _keyword; set => _keyword = value; }
    }
}


