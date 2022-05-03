namespace EWMS.Models.EncryptionFolder
{
    internal class Creator
    {
        public static ICipher GetCipher(CipherType type)
        {
            ICipher cipher = null;

            if (type is CipherType.None)
            {
                return null;
            }
            else if (type is CipherType.Caesar)
            {
                CaesarCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.Polybius)
            {
                PolybiusCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.Vigenere)
            {
                VigenereCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.Nihilist)
            {
                NihilistCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.Fence)
            {
                FenceCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.Bifid)
            {
                BifidCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.Trifid)
            {
                TrifidCipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.ROT13)
            {
                ROT13Cipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.ROT47)
            {
                ROT47Cipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.RC4)
            {
                RC4Cipher tempCipher = new();
                cipher = tempCipher;
            }
            else if (type is CipherType.AES)
            {
                AESCipher tempCipher = new();
                cipher = tempCipher;
            }

            return cipher;
        }

        public static ICipher GetCipher(CipherType type, object param)
        {
            ICipher cipher = GetCipher(type);
            cipher.UpdateParam(param);

            return cipher;
        }
    }
}
