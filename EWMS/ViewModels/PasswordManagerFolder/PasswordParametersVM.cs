using EWMS.Models.PasswordManagerFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class PasswordParametersVM : INotifyPropertyChanged
    {
        private readonly PasswordManagerParameters _passwordManagerParameters;
        public event Action<PasswordManagerParameters> ChildPasswordParametersUpdated;

        private bool _uppercaseLetters;
        public bool UppercaseLetters
        {
            get => _uppercaseLetters;
            set
            {
                _uppercaseLetters = value;

                if (_uppercaseLetters == false && _lowercaseLetters == false && _numbers == false)
                {
                    SimilarCharacters = false;
                }

                OnPropertyChanged();
                _passwordManagerParameters.UppercaseLetters = _uppercaseLetters;
                ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private bool _lowercaseLetters;
        public bool LowercaseLetters
        {
            get => _lowercaseLetters;
            set
            {
                _lowercaseLetters = value;

                if (_uppercaseLetters == false && _lowercaseLetters == false && _numbers == false)
                {
                    SimilarCharacters = false;
                }

                OnPropertyChanged();
                _passwordManagerParameters.LowercaseLetters = _lowercaseLetters;
                ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private bool _numbers;
        public bool Numbers
        {
            get => _numbers;
            set
            {
                _numbers = value;

                if (_uppercaseLetters == false && _lowercaseLetters == false && _numbers == false)
                {
                    SimilarCharacters = false;
                }

                OnPropertyChanged();
                _passwordManagerParameters.Numbers = _numbers;
                ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private bool _symbols;
        public bool Symbols
        {
            get => _symbols;
            set
            {
                _symbols = value;


                if (_symbols == false)
                {
                    AmbiguousCharacters = false;
                }

                OnPropertyChanged();
                _passwordManagerParameters.Symbols = _symbols;
                ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private bool _similarCharacters;
        public bool SimilarCharacters
        {
            get => _similarCharacters;
            set
            {
                if (!(_uppercaseLetters == false && _lowercaseLetters == false && _numbers == false))
                {
                    _similarCharacters = value;
                }
                else
                {
                    _similarCharacters = false;
                }

                OnPropertyChanged();
                _passwordManagerParameters.SimilarCharacters = _similarCharacters;
                ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private bool _ambiguousCharacters;
        public bool AmbiguousCharacters
        {
            get => _ambiguousCharacters;
            set
            {
                if (!(_symbols == false))
                {
                    _ambiguousCharacters = value;
                }
                else
                {
                    _ambiguousCharacters = false;
                }

                OnPropertyChanged();
                _passwordManagerParameters.AmbiguousCharacters = _ambiguousCharacters;
                ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
            }
        }

        private string _passwordLength;
        public string PasswordLength
        {
            get => _passwordLength;
            set
            {
                string text = ValidateCommonInput.ValidateTextToPositiveNumber(value);
                if (ValidateCommonInput.ValidateTextMaxValue(text, _passwordManagerParameters.PasswordMaxLength))
                {
                    _passwordLength = text;
                    OnPropertyChanged();
                    _passwordManagerParameters.PasswordLength = int.Parse(_passwordLength);
                    ChildPasswordParametersUpdated?.Invoke(_passwordManagerParameters);
                }
            }
        }

        private string _passwordMaxLength;
        public string PasswordMaxLength
        {
            get => _passwordMaxLength;
            set
            {
                _passwordMaxLength = value;
                OnPropertyChanged();
            }
        }

        public PasswordParametersVM()
        {
            _passwordManagerParameters = new PasswordManagerParameters();
            PasswordLength = _passwordManagerParameters.PasswordLength.ToString();
            PasswordMaxLength = _passwordManagerParameters.PasswordMaxLength.ToString();
            UppercaseLetters = _passwordManagerParameters.UppercaseLetters;
            LowercaseLetters = _passwordManagerParameters.LowercaseLetters;
            Numbers = _passwordManagerParameters.Numbers;
            Symbols = _passwordManagerParameters.Symbols;
            SimilarCharacters = _passwordManagerParameters.SimilarCharacters;
            AmbiguousCharacters = _passwordManagerParameters.AmbiguousCharacters;
        }

        public PasswordParametersVM(PasswordManagerParameters passwordManagerParameters)
        {
            _passwordManagerParameters = passwordManagerParameters;
            PasswordLength = _passwordManagerParameters.PasswordLength.ToString();
            PasswordMaxLength = _passwordManagerParameters.PasswordMaxLength.ToString();
            UppercaseLetters = _passwordManagerParameters.UppercaseLetters;
            LowercaseLetters = _passwordManagerParameters.LowercaseLetters;
            Numbers = _passwordManagerParameters.Numbers;
            Symbols = _passwordManagerParameters.Symbols;
            SimilarCharacters = _passwordManagerParameters.SimilarCharacters;
            AmbiguousCharacters = _passwordManagerParameters.AmbiguousCharacters;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
