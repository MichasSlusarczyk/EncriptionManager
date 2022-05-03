using EWMS.Models.Containers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class TextParametersVM : INotifyPropertyChanged
    {
        private readonly TextParameters _textParameters;
        public event Action<TextParameters> ChildTextParametersUpdated;

        private bool _uppercase;
        public bool Uppercase
        {
            get => _uppercase;
            set
            {
                _uppercase = value;

                if (_uppercase == true)
                {
                    Lowercase = !_uppercase;
                }

                OnPropertyChanged();
                _textParameters.Uppercase = _uppercase;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _lowercase;
        public bool Lowercase
        {
            get => _lowercase;
            set
            {
                _lowercase = value;

                if (_lowercase == true)
                {
                    Uppercase = !_lowercase;
                }

                OnPropertyChanged();
                _textParameters.Lowercase = _lowercase;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _symbols;
        public bool Symbols
        {
            get => _symbols;
            set
            {
                _symbols = value;

                if (_symbols == true)
                {
                    Ambiguous = _symbols;
                }

                OnPropertyChanged();
                _textParameters.Symbols = _symbols;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _ambiguous;
        public bool Ambiguous
        {
            get => _ambiguous;
            set
            {
                if (_symbols == false)
                {
                    _ambiguous = value;
                }
                else
                {
                    _ambiguous = _symbols;
                }
                OnPropertyChanged();
                _textParameters.AmbiguousSigns = _ambiguous;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _numbers;
        public bool Numbers
        {
            get => _numbers;
            set
            {
                _numbers = value;
                OnPropertyChanged();
                _textParameters.Numbers = _numbers;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _letters;
        public bool Letters
        {
            get => _letters;
            set
            {
                _letters = value;
                OnPropertyChanged();
                _textParameters.Letters = _letters;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _space;
        public bool Space
        {
            get => _space;
            set
            {
                _space = value;
                OnPropertyChanged();
                _textParameters.Space = _space;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        private bool _otherCharacters;
        public bool OtherCharacters
        {
            get => _otherCharacters;
            set
            {
                _otherCharacters = value;
                OnPropertyChanged();
                _textParameters.OtherCharacters = _otherCharacters;
                ChildTextParametersUpdated?.Invoke(_textParameters);
            }
        }

        public TextParametersVM()
        {
            _textParameters = new TextParameters();
            Uppercase = _textParameters.Uppercase;
            Lowercase = _textParameters.Lowercase;
            Symbols = _textParameters.Symbols;
            Ambiguous = _textParameters.AmbiguousSigns;
            Numbers = _textParameters.Numbers;
            Letters = _textParameters.Letters;
            Space = _textParameters.Space;
            OtherCharacters = _textParameters.OtherCharacters;
        }

        public TextParametersVM(TextParameters textParameters)
        {
            _textParameters = textParameters;
            Uppercase = _textParameters.Uppercase;
            Lowercase = _textParameters.Lowercase;
            Symbols = _textParameters.Symbols;
            Ambiguous = _textParameters.AmbiguousSigns;
            Numbers = _textParameters.Numbers;
            Letters = _textParameters.Letters;
            Space = _textParameters.Space;
            OtherCharacters = _textParameters.OtherCharacters;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
