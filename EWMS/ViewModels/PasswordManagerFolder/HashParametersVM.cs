using EWMS.Models.PasswordManagerFolder;
using EWMS.Models.Validation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class HashParametersVM : INotifyPropertyChanged
    {
        private readonly PasswordManagerParameters _passwordManagerParameters;
        public event Action<PasswordManagerParameters> ChildHashParametersUpdated;

        private bool _addSalt;
        public bool AddSalt
        {
            get => _addSalt;
            set
            {
                _addSalt = value;
                OnPropertyChanged();
                _passwordManagerParameters.AddSalt = _addSalt;
                ChildHashParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private string _saltLength;
        public string SaltLength
        {
            get => _saltLength;
            set
            {
                string text = ValidateCommonInput.ValidateTextToPositiveNumber(value);
                if (ValidateCommonInput.ValidateTextMaxValue(text, _passwordManagerParameters.SaltMaxLength))
                {
                    _saltLength = text;
                    OnPropertyChanged();
                    _passwordManagerParameters.SaltLength = int.Parse(_saltLength);
                    ChildHashParametersUpdated?.Invoke(_passwordManagerParameters);
                }
            }
        }

        private string _saltMaxLength;
        public string SaltMaxLength
        {
            get => _saltMaxLength;
            set
            {
                _saltMaxLength = value;
                OnPropertyChanged();
            }
        }

        private string _hashLength;
        public string HashLength
        {
            get => _hashLength;
            set
            {
                _hashLength = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Hash> _hashTypeList;
        public ObservableCollection<Hash> HashTypeList
        {
            get => _hashTypeList;
            set
            {
                _hashTypeList = value;
                OnPropertyChanged();
            }
        }

        private Hash _selectedHashType;
        public Hash SelectedHashType
        {
            get
            {
                if (_selectedHashType == null)
                {
                    _selectedHashType = _hashTypeList.FirstOrDefault();
                    OnPropertyChanged();
                }
                return _selectedHashType;
            }
            set
            {
                _selectedHashType = value;
                _passwordManagerParameters.HashType = _selectedHashType.HashTypeEnum;
                OnPropertyChanged();
            }
        }

        public HashParametersVM()
        {
            _passwordManagerParameters = new PasswordManagerParameters();
            AddSalt = _passwordManagerParameters.AddSalt;
            SaltLength = _passwordManagerParameters.SaltLength.ToString();
            SaltMaxLength = _passwordManagerParameters.SaltMaxLength.ToString();
            _hashTypeList = new ObservableCollection<Hash>();
            InitialzeHashTypeList();
        }

        public HashParametersVM(PasswordManagerParameters passwordManagerParameters)
        {
            _passwordManagerParameters = passwordManagerParameters;
            AddSalt = _passwordManagerParameters.AddSalt;
            SaltLength = _passwordManagerParameters.SaltLength.ToString();
            SaltMaxLength = _passwordManagerParameters.SaltMaxLength.ToString();
            _hashTypeList = new ObservableCollection<Hash>();
            InitialzeHashTypeList();
        }


        private void InitialzeHashTypeList()
        {
            foreach (HashType hash in (HashType[])Enum.GetValues(typeof(HashType)))
            {
                _hashTypeList.Add(new Hash() { HashTypeName = hash.ToString(), HashTypeEnum = hash });
            }
        }

        public void UpdateHashLength(int hashLength)
        {
            HashLength = hashLength.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class Hash : INotifyPropertyChanged
    {
        private string _hashTypeName;
        public string HashTypeName
        {
            get => _hashTypeName;
            set
            {
                _hashTypeName = value;
                OnPropertyChanged();
            }
        }

        private HashType _hashTypeEnum;
        public HashType HashTypeEnum
        {
            get => _hashTypeEnum;
            set => _hashTypeEnum = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
