using EWMS.Models.Containers;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal interface ICipher
    {
        public abstract List<byte> Encrypt(Data input);
        public abstract List<byte> Decrypt(Data input);
        public abstract void UpdateParam(object param);
        public abstract void ResetParam();

        public abstract string CheckparamValidation();
        public abstract string CheckparamValidation(Data input);
        public abstract CipherClass ReturnCipherClass();
        public abstract CipherType ReturnCipherType();
        public abstract string ReturnCipherName();
        public abstract string ReturnCipherDescription();
    }
}
