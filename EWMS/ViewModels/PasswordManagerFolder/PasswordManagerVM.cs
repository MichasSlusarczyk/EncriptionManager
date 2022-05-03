using EWMS.Models.PasswordManagerFolder;
using EWMS.Models.Validation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EWMS.ViewModels
{
    internal class PasswordManagerVM : INotifyPropertyChanged
    {
        private readonly PasswordManager _passwordManager;

        public ICommand GoToPasswordParametersUCCommand { get; set; }
        public ICommand GoToHashParametersUCCommand { get; set; }
        public ICommand GeneratePasswordCommand { get; set; }
        public ICommand GenerateSaltCommand { get; set; }
        public ICommand GenerateHashCommand { get; set; }
        public ICommand CopyPasswordToClipboardCommand { get; set; }
        public ICommand CopySaltToClipboardCommand { get; set; }
        public ICommand CopyHashToClipboardCommand { get; set; }


        private object _passwordHashParametersVM;

        public object PasswordHashParametersVM
        {
            get => _passwordHashParametersVM;

            set { _passwordHashParametersVM = value; OnPropertyChanged(nameof(PasswordHashParametersVM)); }
        }

        private object _passwordParametersVM;

        public object PasswordParametersVM
        {
            get => _passwordParametersVM;

            set { _passwordParametersVM = value; OnPropertyChanged(nameof(PasswordParametersVM)); }
        }

        private object _hashParametersVM;

        public object HashParametersVM
        {
            get => _hashParametersVM;

            set { _hashParametersVM = value; OnPropertyChanged(nameof(HashParametersVM)); }
        }

        private string _password;
        public string Password
        {
            get => _password;

            set
            {
                _password = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), _passwordManager.PasswordParameters.PasswordMaxLength);
                _passwordManager.Password = _password;
                Hash = string.Empty;
                OnPropertyChanged();
            }
        }

        private string _salt;
        public string Salt
        {
            get => _salt;

            set
            {
                _salt = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), _passwordManager.PasswordParameters.SaltMaxLength);
                _passwordManager.PasswordSalt = _salt;
                Hash = string.Empty;
                OnPropertyChanged();
            }
        }

        private string _hash;
        public string Hash
        {
            get => _hash;

            set
            {
                _hash = value;
                _passwordManager.PasswordHash = _hash;
                OnPropertyChanged();
            }
        }

        public PasswordManagerVM()
        {
            _passwordManager = new PasswordManager();
            PasswordParametersVM = new PasswordParametersVM(_passwordManager.PasswordParameters);
            HashParametersVM = new HashParametersVM(_passwordManager.PasswordParameters);
            Password = _passwordManager.Password;
            Salt = _passwordManager.PasswordSalt;
            Hash = _passwordManager.PasswordHash;
            PasswordHashParametersVM = PasswordParametersVM;
            ((PasswordParametersVM)PasswordHashParametersVM).ChildPasswordParametersUpdated += PasswordParametersUpdated;
            GoToPasswordParametersUCCommand = new RelayCommand(GoToPasswordParametersUC);
            GoToHashParametersUCCommand = new RelayCommand(GoToHashParametersUC);
            GeneratePasswordCommand = new RelayCommand(GeneratePassword);
            GenerateSaltCommand = new RelayCommand(GenerateSalt);
            GenerateHashCommand = new RelayCommand(GenerateHash);
            CopyPasswordToClipboardCommand = new RelayCommand(CopyPasswordToClipboard);
            CopySaltToClipboardCommand = new RelayCommand(CopySaltToClipboard);
            CopyHashToClipboardCommand = new RelayCommand(CopyHashToClipboard);
        }

        private void GoToPasswordParametersUC(object obj)
        {
            PasswordHashParametersVM = PasswordParametersVM;
            ((PasswordParametersVM)PasswordHashParametersVM).ChildPasswordParametersUpdated += PasswordParametersUpdated;
        }

        private void GoToHashParametersUC(object obj)
        {
            PasswordHashParametersVM = HashParametersVM;
            ((HashParametersVM)PasswordHashParametersVM).ChildHashParametersUpdated += HashParametersUpdated;
            ((HashParametersVM)PasswordHashParametersVM).UpdateHashLength(_hash.Length);
        }

        private void GeneratePassword(object obj)
        {
            if (_passwordManager.CheckGeneratedPasswordLength() && _passwordManager.PasswordParameters.PasswordLength != 0)
            {
                _passwordManager.GeneratePassword();
                Password = _passwordManager.Password;
            }
            else
            {
                _passwordManager.Password = string.Empty;
                Password = string.Empty;
                DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, ValidatePasswordManager.ValidateGeneratePassword(_passwordManager));
            }
        }

        private void GenerateSalt(object obj)
        {
            if (_passwordManager.CheckGeneratedSaltLength() && _passwordManager.PasswordParameters.SaltLength != 0)
            {
                _passwordManager.GenerateSalt();
                Salt = _passwordManager.PasswordSalt;
            }
            else
            {
                _passwordManager.PasswordSalt = string.Empty;
                Salt = string.Empty;
                DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, ValidatePasswordManager.ValidateGenerateSalt(_passwordManager));
            }
        }

        private void GenerateHash(object obj)
        {
            if (_passwordManager.PasswordParameters.HashType != HashType.None && _passwordManager.Password != string.Empty)
            {
                _passwordManager.GenerateHash();
                Hash = _passwordManager.PasswordHash;
                if (_passwordManager.PasswordSalt == string.Empty && _passwordManager.PasswordParameters.AddSalt == true)
                {
                    Salt = _passwordManager.PasswordSalt;
                }
            }
            else
            {
                _passwordManager.PasswordHash = string.Empty;
                Hash = string.Empty;
                DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, ValidatePasswordManager.ValidateGenerateHash(_passwordManager));
            }

            if (PasswordHashParametersVM.GetType() == typeof(HashParametersVM))
            {
                ((HashParametersVM)PasswordHashParametersVM).UpdateHashLength(_hash.Length);
            }
        }

        private void CopyPasswordToClipboard(object obj)
        {
            if (Password != string.Empty)
            {
                Clipboard.SetText(Password);
            }
        }

        private void CopySaltToClipboard(object obj)
        {
            if (Salt != string.Empty)
            {
                Clipboard.SetText(Salt);
            }
        }

        private void CopyHashToClipboard(object obj)
        {
            if (Salt != string.Empty)
            {
                Clipboard.SetText(Salt);
            }
        }

        private void HashParametersUpdated(PasswordManagerParameters passwordManagerParameters)
        {
            _passwordManager.PasswordParameters.AddSalt = passwordManagerParameters.AddSalt;
            _passwordManager.PasswordParameters.SaltLength = passwordManagerParameters.SaltLength;
            _passwordManager.PasswordParameters.HashType = passwordManagerParameters.HashType;
        }

        private void PasswordParametersUpdated(PasswordManagerParameters passwordManagerParameters)
        {
            _passwordManager.PasswordParameters.PasswordLength = passwordManagerParameters.PasswordLength;
            _passwordManager.PasswordParameters.UppercaseLetters = passwordManagerParameters.UppercaseLetters;
            _passwordManager.PasswordParameters.LowercaseLetters = passwordManagerParameters.LowercaseLetters;
            _passwordManager.PasswordParameters.Numbers = passwordManagerParameters.Numbers;
            _passwordManager.PasswordParameters.Symbols = passwordManagerParameters.Symbols;
            _passwordManager.PasswordParameters.SimilarCharacters = passwordManagerParameters.SimilarCharacters;
            _passwordManager.PasswordParameters.AmbiguousCharacters = passwordManagerParameters.AmbiguousCharacters;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
