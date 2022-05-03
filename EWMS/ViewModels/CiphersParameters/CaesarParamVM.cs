using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class CaesarParamVM : INotifyPropertyChanged
    {
        private CaesarParams _cipherParameters;
        public event Action<CaesarParams> ChildCipherParametersUpdated;

        private readonly CipherType _cipherType;

        private string _shift;
        public string Shift
        {
            get => _shift;
            set
            {
                _shift = ValidateCommonInput.ValidateTextMaxLength(ValidateCommonInput.ValidateTextToNumber(value), 3);
                OnPropertyChanged();
                _cipherParameters.Shift = int.Parse(_shift);

                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        public CaesarParamVM()
        {
            _cipherType = CipherType.Caesar;
            _cipherParameters = new CaesarParams();
            Shift = _cipherParameters.Shift.ToString();
        }

        public CaesarParamVM(CaesarParams cipherParameters)
        {
            _cipherType = CipherType.Caesar;
            _cipherParameters = cipherParameters;
            Shift = _cipherParameters.Shift.ToString();
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
            _cipherParameters = (CaesarParams)param;
        }
    }
}