using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class FenceParamVM : INotifyPropertyChanged
    {
        private FenceParams _cipherParameters;
        public event Action<FenceParams> ChildCipherParametersUpdated;

        private readonly CipherType _cipherType;

        private string _fenceHeight;
        public string FenceHeight
        {
            get => _fenceHeight;
            set
            {
                _fenceHeight = ValidateCommonInput.ValidateTextMaxLength(ValidateCommonInput.ValidateTextToPositiveNumber(value), 5);
                _cipherParameters.FenceHeight = int.Parse(_fenceHeight);
                OnPropertyChanged();
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
                
            }
        }

        public FenceParamVM()
        {
            _cipherType = CipherType.Fence;
            _cipherParameters = new FenceParams();
            FenceHeight = _cipherParameters.FenceHeight.ToString();
        }

        public FenceParamVM(FenceParams cipherParameters)
        {
            _cipherType = CipherType.Fence;
            _cipherParameters = cipherParameters;
            FenceHeight = _cipherParameters.FenceHeight.ToString();
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
            _cipherParameters = (FenceParams)param;
        }
    }
}