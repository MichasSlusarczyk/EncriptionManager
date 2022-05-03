using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace EWMS.ViewModels
{
    internal class AESParamVM : INotifyPropertyChanged
    {
        private AESParams _cipherParameters;
        public event Action<AESParams> ChildCipherParametersUpdated;

        private readonly CipherType _cipherType;

        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), 32);
                OnPropertyChanged();
                _cipherParameters.Key = _key;
                KeyLength = _key.Length.ToString();
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        private string _IV;
        public string IV
        {
            get => _IV;
            set
            {
                _IV = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value),16);
                OnPropertyChanged();
                _cipherParameters.InitializationVector = _IV;
                IVLength = _IV.Length.ToString();
                ChildCipherParametersUpdated?.Invoke(_cipherParameters);
            }
        }

        private string _keyLength;
        public string KeyLength
        {
            get => _keyLength;
            set
            {
                _keyLength = value;
                OnPropertyChanged();
            }
        }

        private string _IVLength;
        public string IVLength
        {
            get => _IVLength;
            set
            {
                _IVLength = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CMode> _cipherModeList;
        public ObservableCollection<CMode> CipherModeList
        {
            get => _cipherModeList;
            set
            {
                _cipherModeList = value;
                OnPropertyChanged();
            }
        }

        private CMode _selectedCipherMode;
        public CMode SelectedCipherMode
        {
            get
            {
                if (_selectedCipherMode == null)
                {
                    _selectedCipherMode = _cipherModeList.FirstOrDefault();
                    OnPropertyChanged();
                }
                return _selectedCipherMode;
            }
            set
            {
                _selectedCipherMode = value;
                _cipherParameters.CipherMode = _selectedCipherMode.CipherModeEnum;
                OnPropertyChanged();
            }
        }

        public AESParamVM()
        {
            _cipherType = CipherType.AES;
            _cipherParameters = new AESParams();
            Key = _cipherParameters.Key;
            IV = _cipherParameters.InitializationVector;
            InitialzeCipherModeList();
        }

        public AESParamVM(AESParams cipherParameters)
        {
            _cipherType = CipherType.AES;
            _cipherParameters = cipherParameters;
            Key = _cipherParameters.Key;
            IV = _cipherParameters.InitializationVector;
            InitialzeCipherModeList();
        }

        private void InitialzeCipherModeList()
        {
            _cipherModeList = new ObservableCollection<CMode>();

            foreach (CipherMode mode in _cipherParameters.PossibleCipherModes)
            {
                _cipherModeList.Add(new CMode() { CipherModeName = mode.ToString(), CipherModeEnum = mode });
            }
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
            _cipherParameters = (AESParams)param;
        }
    }

    internal class CMode : INotifyPropertyChanged
    {
        private string _cipherModeName;
        public string CipherModeName
        {
            get => _cipherModeName;
            set
            {
                _cipherModeName = value;
                OnPropertyChanged();
            }
        }

        private CipherMode _cipherModeEnum;
        public CipherMode CipherModeEnum
        {
            get => _cipherModeEnum;
            set => _cipherModeEnum = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
