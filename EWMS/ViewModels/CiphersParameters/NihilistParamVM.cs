using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class NihilistParamVM : INotifyPropertyChanged
    {
        private NihilstParams _cipherParameters;
        public event Action<NihilstParams> ChildCipherParametersUpdated;

        private readonly CipherType _cipherType;

        private string _keyword1;
        public string Keyword1
        {
            get => _keyword1;
            set
            {
                _keyword1 = ValidateCommonInput.ValidateTextMaxLength(ValidateCommonInput.ValidateTextToLettersOnly(value), 10000);
                OnPropertyChanged();
                _cipherParameters.Keyword1 = _keyword1;
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        private string _keyword2;
        public string Keyword2
        {
            get => _keyword2;
            set
            {
                _keyword2 = ValidateCommonInput.ValidateTextMaxLength(ValidateCommonInput.ValidateTextToLettersOnly(value), 10000);
                OnPropertyChanged();
                _cipherParameters.Keyword2 = _keyword2;
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        public NihilistParamVM()
        {
            _cipherType = CipherType.Nihilist;
            _cipherParameters = new NihilstParams();
            Keyword1 = _cipherParameters.Keyword1;
            Keyword2 = _cipherParameters.Keyword2;
        }

        public NihilistParamVM(NihilstParams cipherParameters)
        {
            _cipherType = CipherType.Nihilist;
            _cipherParameters = cipherParameters;
            Keyword1 = _cipherParameters.Keyword1;
            Keyword2 = _cipherParameters.Keyword2;
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
            _cipherParameters = (NihilstParams)param;
        }
    }
}
