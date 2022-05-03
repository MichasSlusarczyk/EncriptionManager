using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class VigenereParamVM : INotifyPropertyChanged
    {
        private VigenereParams _cipherParameters;
        public event Action<VigenereParams> ChildCipherParametersUpdated;

        private readonly CipherType _cipherType;

        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set
            {
                _keyword = ValidateCommonInput.ValidateTextMaxLength(ValidateCommonInput.ValidateTextToLettersOnly(value), 10000);
                OnPropertyChanged();
                _cipherParameters.Keyword = _keyword;
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        public VigenereParamVM()
        {
            _cipherType = CipherType.Vigenere;
            _cipherParameters = new VigenereParams();
            Keyword = _cipherParameters.Keyword;
        }

        public VigenereParamVM(VigenereParams cipherParameters)
        {
            _cipherType = CipherType.Vigenere;
            _cipherParameters = cipherParameters;
            Keyword = _cipherParameters.Keyword;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CipherType ReturnCipherType()
        {
            return _cipherType;
        }

        public void UpdateParam(object param)
        {
            _cipherParameters = (VigenereParams)param;
        }
    }
}
