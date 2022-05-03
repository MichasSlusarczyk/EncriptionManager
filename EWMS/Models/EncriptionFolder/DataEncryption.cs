using EWMS.Models.Containers;
using System.Collections.Generic;

namespace EWMS.Models.EncryptionFolder
{
    internal class DataEncryption
    {
        private EncryptDecrypt _enDe;
        private Data _inputBinE;
        private List<byte> _outputBinE;
        private Data _inputBinD;
        private List<byte> _outputBinD;
        private ICipher _cipher;

        public ICipher Cipher { get => _cipher; set => _cipher = value; }
        public EncryptDecrypt EnDe { get => _enDe; set => _enDe = value; }
        public Data InputBinE { get => _inputBinE; set => _inputBinE = value; }
        public List<byte> OutputBinE { get => _outputBinE; set => _outputBinE = value; }
        public Data InputBinD { get => _inputBinD; set => _inputBinD = value; }
        public List<byte> OutputBinD { get => _outputBinD; set => _outputBinD = value; }

        public DataEncryption()
        {
            _inputBinE = new();
            _inputBinD = new();
            _outputBinE = new();
            _outputBinD = new();
            EnDe = EncryptDecrypt.None;
            _cipher = null;
        }

        public void Encryption()
        {
            if (EnDe == EncryptDecrypt.Encryption && _inputBinE.DataModified.Count != 0)
            {
                Encrypt();
            }
            else if (EnDe == EncryptDecrypt.Decryption && _inputBinD.DataModified.Count != 0)
            {
                Decrypt();
            }
        }

        private void Encrypt()
        {
            _outputBinE = _cipher.Encrypt(_inputBinE);
        }

        private void Decrypt()
        {
            _outputBinD = _cipher.Decrypt(_inputBinD);
        }
    }
}
