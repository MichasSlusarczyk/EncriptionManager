using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace EWMS.Models.EncryptionFolder
{
    internal class AESCipher : ICipher
    {
        private AESParams _AESParam;
        public AESParams AESParam { get => _AESParam; set => _AESParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        public AESCipher()
        {
            _cipherName = "AES Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Any file format during encryption and dedicated file format during decryption. The maximum size of the input file is 100kB.\n\n" +
                "PARAMETERS:\n" +
                "Key - text with letters in the decimal ASCII range 0-255 with permissible lengths of 16, 25 and 32 bytes.\n" +
                "Initialization vector - text with letters in the decimal ASCII range 0-255 with length of 16 bytes.\n" +
                "Cipher mode - selectable cipher modes.\n\n" +
                "OUTPUT:\n" +
                "A dedicated file format during encryption and any file format during decryption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The symmetric block cipher adopted by NIST (National Institute of Standards and Technology) as the FIPS-197 standard following a competition announced in 1997. In 2001, it was adopted as a standard. AES is based on the Rijndael algorithm developed by Belgian cryptographers Joan Daemen and Vincent Rijmen. They presented their proposal for the NIST Institution cipher in the competition announced.Rijndael is a family of ciphers with different key lengths and different block sizes. AES is based on a principle called the substitution - permutation network.It shows high speed of work both in the case of computer hardware and software.Unlike its predecessor, the DES algorithm, AES does not use the Feistel Network.AES has a specific block size of 128 bits, and the key size is 128, 192, or 256 bits.The substitution function has a very original design which makes this algorithm immune to known attacks of differential and linear cryptanalysis.";
            _cipherClass = CipherClass.Block;
            _cipherType = CipherType.AES;
            _AESParam = new();
        }

        public AESCipher(object param)
        {
            _cipherName = "Bifid Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Any file format during encryption and dedicated file format during decryption. The maximum size of the input file is 100kB.\n\n" +
                "PARAMETERS:\n" +
                "Key - text with letters in the decimal ASCII range 0-255 with permissible lengths of 16, 25 and 32 bytes.\n" +
                "Initialization vector - text with letters in the decimal ASCII range 0-255 with length of 16 bytes.\n" +
                "Cipher mode - selectable cipher modes.\n\n" +
                "OUTPUT:\n" +
                "A dedicated file format during encryption and any file format during decryption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The symmetric block cipher adopted by NIST (National Institute of Standards and Technology) as the FIPS-197 standard following a competition announced in 1997. In 2001, it was adopted as a standard. AES is based on the Rijndael algorithm developed by Belgian cryptographers Joan Daemen and Vincent Rijmen. They presented their proposal for the NIST Institution cipher in the competition announced.Rijndael is a family of ciphers with different key lengths and different block sizes. AES is based on a principle called the substitution - permutation network.It shows high speed of work both in the case of computer hardware and software.Unlike its predecessor, the DES algorithm, AES does not use the Feistel Network.AES has a specific block size of 128 bits, and the key size is 128, 192, or 256 bits.The substitution function has a very original design which makes this algorithm immune to known attacks of differential and linear cryptanalysis.";
            _cipherClass = CipherClass.Block;
            _cipherType = CipherType.AES;
            _AESParam = new();
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            AESParam = (AESParams)param;
        }

        public List<byte> Encrypt(Data input)
        {
            byte[] output;
            byte[] inputByteArray = ByteListByteArrayConverter.ByteListToByteArray(input.DataModified);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = _AESParam.CipherMode;
                aes.KeySize = _AESParam.Key.Length * 8;
                aes.Key = ByteArrayToStringConverter.StringToByteArray(_AESParam.Key);
                aes.IV = ByteArrayToStringConverter.StringToByteArray(_AESParam.InitializationVector);

                try
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    output = encryptor.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                }
                catch (Exception)
                {
                    output = new byte[] { 0 };
                }
            }

            return ByteListByteArrayConverter.ByteArrayToByteList(output);
        }

        public List<byte> Decrypt(Data input)
        {
            byte[] output;
            byte[] inputByteArray = ByteListByteArrayConverter.ByteListToByteArray(input.DataModified);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = _AESParam.CipherMode;
                aes.KeySize = _AESParam.Key.Length * 8;
                aes.Key = ByteArrayToStringConverter.StringToByteArray(_AESParam.Key);
                aes.IV = ByteArrayToStringConverter.StringToByteArray(_AESParam.InitializationVector);

                try
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    output = decryptor.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                }
                catch (Exception)
                {
                    output = new byte[] { 0 };
                }

            }

            return ByteListByteArrayConverter.ByteArrayToByteList(output);
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

            if (_AESParam.Key.Length != 16 && _AESParam.Key.Length != 24 && _AESParam.Key.Length != 32)
            {
                message += "Key length should be equal to 16, 24 or 32!\n";
            }

            if (_AESParam.InitializationVector.Length != 16)
            {
                message += "Initialization vector length should be equal 16!";
            }

            return message;
        }

        public string CheckparamValidation(Data input)
        {
            return string.Empty;
        }

        public void ResetParam()
        {
            _AESParam = new();
        }
    }

    internal class AESParams
    {
        private string _key;
        private string _initializationVector;
        private CipherMode _cipherMode;
        private CipherMode[] _possibleCipherModes;

        public AESParams()
        {
            _key = string.Empty;
            _initializationVector = string.Empty;
            _cipherMode = CipherMode.CBC;
            _possibleCipherModes = new CipherMode[] { CipherMode.CBC, CipherMode.ECB, CipherMode.CFB };
        }
        public string Key { get => _key; set => _key = value; }
        public string InitializationVector { get => _initializationVector; set => _initializationVector = value; }
        public CipherMode CipherMode { get => _cipherMode; set => _cipherMode = value; }
        public CipherMode[] PossibleCipherModes { get => _possibleCipherModes; set => _possibleCipherModes = value; }
    }
}



