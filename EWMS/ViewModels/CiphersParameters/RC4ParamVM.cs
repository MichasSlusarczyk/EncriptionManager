using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class RC4ParamVM : INotifyPropertyChanged
    {
        private RC4Params _cipherParameters;
        public event Action<RC4Params> ChildCipherParametersUpdated;

        private readonly CipherType _cipherType;

        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set
            {
                _keyword = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), 256);
                OnPropertyChanged();
                _cipherParameters.Keyword = _keyword;
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        public RC4ParamVM()
        {
            _cipherType = CipherType.RC4;
            _cipherParameters = new RC4Params();
            Keyword = _cipherParameters.Keyword;
        }

        public RC4ParamVM(RC4Params cipherParameters)
        {
            _cipherType = CipherType.RC4;
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
            _cipherParameters = (RC4Params)param;
        }
    }
}
