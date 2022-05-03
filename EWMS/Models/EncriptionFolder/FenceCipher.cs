using EWMS.Models.Containers;
using System;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class FenceCipher : ICipher
    {
        private FenceParams _fenceParam;
        public FenceParams FenceParam { get => _fenceParam; set => _fenceParam = value; }

        private string _cipherName;
        public string CipherName { get => _cipherName; set => _cipherName = value; }

        private string _cipherDescription;
        public string CipherDescription { get => _cipherDescription; set => _cipherDescription = value; }

        private CipherClass _cipherClass;
        internal CipherClass CipherClass { get => _cipherClass; set => _cipherClass = value; }

        private CipherType _cipherType;
        internal CipherType CipherType { get => _cipherType; set => _cipherType = value; }

        public FenceCipher()
        {
            _cipherName = "Fence Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "Fence height - number from 1 to message length.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The Fence Cipher is a transposition cipher. It works by rearranging the order of letters, based on the simplified shape of a wooden fence.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Fence;
            _fenceParam = new();
        }

        public FenceCipher(object param)
        {
            _cipherName = "Fence Cipher";
            _cipherDescription =
                "INPUT:\n" +
                "Unlimited text where only characters in the range a-Z are encrypted/decypted. The maximum size of the input text is 10kB.\n\n" +
                "PARAMETERS:\n" +
                "Fence height - number from 1 to message length.\n\n" +
                "OUTPUT:\n" +
                "Encrypted/decrypted text with rewritten characters not eligible for encryption.\n\n" +
                "SHORT DESCRIPTION:\n" +
                "The Fence Cipher is a transposition cipher. It works by rearranging the order of letters, based on the simplified shape of a wooden fence.";
            _cipherClass = CipherClass.Classic;
            _cipherType = CipherType.Fence;
            UpdateParam(param);
        }

        public void UpdateParam(object param)
        {
            FenceParam = (FenceParams)param;
        }

        public List<byte> Encrypt(Data input)
        {
            List<byte> output = new();

            List<byte> textToEncrypt = new();
            List<byte> encryptedText = new();

            int fanceHeight = FenceParam.FenceHeight;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    textToEncrypt.Add(input.DataModified[i]);
                }
            }

            int textToEncryptLength = textToEncrypt.Count;

            byte[,] fence = new byte[fanceHeight, textToEncryptLength];

            fence.Initialize();

            int rowIterator = 0;
            bool isGoingDown = true;

            if (fanceHeight <= 0 || fanceHeight > textToEncryptLength)
            {
                return new();
            }
            else if (fanceHeight == 1)
            {
                for (int i = 0; i < textToEncryptLength; i++)
                {
                    encryptedText.Add(textToEncrypt[i]);
                }
            }
            else
            {
                for (int i = 0; i < textToEncryptLength; i++)
                {
                    fence[rowIterator, i] = textToEncrypt[i];

                    if (isGoingDown)
                    {
                        rowIterator++;
                        if (rowIterator == (fanceHeight - 1))
                        {
                            isGoingDown = false;
                        }
                    }
                    else
                    {
                        rowIterator--;
                        if (rowIterator == 0)
                        {
                            isGoingDown = true;
                        }
                    }
                }

                for (int i = 0; i < fanceHeight; i++)
                {
                    for (int j = 0; j < textToEncryptLength; j++)
                    {
                        if (fence[i, j] != 0)
                        {
                            encryptedText.Add(fence[i, j]);
                        }
                    }
                }
            }

            int it = 0;
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

            List<byte> textToDecrypt = new();
            List<byte> decryptedText = new();

            int fanceHeight = FenceParam.FenceHeight;

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    textToDecrypt.Add(input.DataModified[i]);
                }
            }

            int textToDecryptLength = textToDecrypt.Count;

            byte[,] fence = new byte[fanceHeight, textToDecryptLength];

            fence.Initialize();

            int rowIterator = 0;
            bool isGoingDown = true;
            int it;
            if (fanceHeight <= 0 || fanceHeight > textToDecryptLength)
            {
                return new();
            }
            else if (fanceHeight == 1)
            {
                for (int i = 0; i < textToDecryptLength; i++)
                {
                    decryptedText.Add(textToDecrypt[i]);
                }
            }
            else
            {

                for (int i = 0; i < textToDecryptLength; i++)
                {
                    fence[rowIterator, i] = 1;

                    if (isGoingDown)
                    {
                        rowIterator++;
                        if (rowIterator == (fanceHeight - 1))
                        {
                            isGoingDown = false;
                        }
                    }
                    else
                    {
                        rowIterator--;
                        if (rowIterator == 0)
                        {
                            isGoingDown = true;
                        }
                    }
                }


                it = 0;
                for (int i = 0; i < fanceHeight; i++)
                {
                    for (int j = 0; j < textToDecryptLength; j++)
                    {
                        if (fence[i, j] == 1)
                        {
                            fence[i, j] = textToDecrypt[it];
                            it++;
                        }
                    }
                }

                for (int j = 0; j < textToDecryptLength; j++)
                {
                    for (int i = 0; i < fanceHeight; i++)
                    {
                        if (fence[i, j] != 0)
                        {
                            decryptedText.Add(fence[i, j]);
                        }
                    }
                }
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

            List<byte> textToEncrypt = new();

            for (int i = 0; i < input.DataModified.Count; i++)
            {
                char check = Convert.ToChar(Convert.ToChar(input.DataModified[i]).ToString().ToUpper());
                if (check >= 'A' && check <= 'Z')
                {
                    textToEncrypt.Add(input.DataModified[i]);
                }
            }

            int textToEncryptLength = textToEncrypt.Count;

            string message = string.Empty;

            if (_fenceParam.FenceHeight <= 0 || _fenceParam.FenceHeight > textToEncryptLength)
            {
                message += "The fence height should be in the range from 1 to " + textToEncryptLength.ToString() + "!";
            }

            return message;
        }

        public void ResetParam()
        {
            _fenceParam = new();
        }
    }

    internal class FenceParams
    {
        private int _fenceHeight;

        public FenceParams()
        {
            _fenceHeight = 0;
        }

        public int FenceHeight { get => _fenceHeight; set => _fenceHeight = value; }

    }
}



