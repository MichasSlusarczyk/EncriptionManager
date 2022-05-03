using EWMS.Models.SteganographyFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class SteganographyParametersVM : INotifyPropertyChanged
    {
        private readonly SteganographyParameters _steganographyParameters;
        public event Action<SteganographyParameters> ChildSteganographyParametersUpdated;

        private bool _dimR;
        public bool DimR
        {
            get => _dimR;
            set
            {
                _dimR = value;
                OnPropertyChanged();
                _steganographyParameters.DimToUse[0] = _dimR;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _dimG;
        public bool DimG
        {
            get => _dimG;
            set
            {
                _dimG = value;
                OnPropertyChanged();
                _steganographyParameters.DimToUse[1] = _dimG;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _dimB;
        public bool DimB
        {
            get => _dimB;
            set
            {
                _dimB = value;
                OnPropertyChanged();
                _steganographyParameters.DimToUse[2] = _dimB;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit0;
        public bool Bit0
        {
            get => _bit0;
            set
            {
                _bit0 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[0] = _bit0;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit1;
        public bool Bit1
        {
            get => _bit1;
            set
            {
                _bit1 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[1] = _bit1;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit2;
        public bool Bit2
        {
            get => _bit2;
            set
            {
                _bit2 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[2] = _bit2;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit3;
        public bool Bit3
        {
            get => _bit3;
            set
            {
                _bit3 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[3] = _bit3;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit4;
        public bool Bit4
        {
            get => _bit4;
            set
            {
                _bit4 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[4] = _bit4;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit5;
        public bool Bit5
        {
            get => _bit5;
            set
            {
                _bit5 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[5] = _bit5;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit6;
        public bool Bit6
        {
            get => _bit6;
            set
            {
                _bit6 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[6] = _bit6;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private bool _bit7;
        public bool Bit7
        {
            get => _bit7;
            set
            {
                _bit7 = value;
                OnPropertyChanged();
                _steganographyParameters.BitsToUse[7] = _bit7;
                ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
            }
        }

        private string _decryptionDisplayLength;
        public string DecryptionDisplayLength
        {
            get => _decryptionDisplayLength;
            set
            {
                string text = ValidateCommonInput.ValidateTextToPositiveNumber(value);

                if (ValidateCommonInput.ValidateTextMaxValue(text, _steganographyParameters.SpaceToRead))
                {
                    _decryptionDisplayLength = text;
                    OnPropertyChanged();

                    _steganographyParameters.DecryptionDisplayLength = int.Parse(_decryptionDisplayLength);

                    ChildSteganographyParametersUpdated?.Invoke(_steganographyParameters);
                }
            }
        }

        private string _spaceToHide;
        public string SpaceToHide
        {
            get => _spaceToHide;
            set
            {
                _spaceToHide = value;
                OnPropertyChanged();
                _steganographyParameters.SpaceToHide = int.Parse(_spaceToHide);
            }
        }

        private string _messageLength;
        public string MessageLength
        {
            get => _messageLength;
            set
            {
                _messageLength = value;
                OnPropertyChanged();
                _steganographyParameters.MessageLength = int.Parse(_messageLength);
            }
        }

        private string _spaceToRead;
        public string SpaceToRead
        {
            get => _spaceToRead;
            set
            {
                _spaceToRead = value;
                OnPropertyChanged();
                _steganographyParameters.SpaceToRead = int.Parse(_spaceToRead);
            }
        }

        public SteganographyParametersVM()
        {
            _steganographyParameters = new SteganographyParameters();

            MessageLength = _steganographyParameters.MessageLength.ToString();
            SpaceToHide = _steganographyParameters.SpaceToHide.ToString();
            DecryptionDisplayLength = _steganographyParameters.DecryptionDisplayLength.ToString();
            SpaceToRead = _steganographyParameters.SpaceToRead.ToString();
            DimR = _steganographyParameters.DimToUse[0];
            DimG = _steganographyParameters.DimToUse[1];
            DimB = _steganographyParameters.DimToUse[2];
            Bit0 = _steganographyParameters.BitsToUse[0];
            Bit1 = _steganographyParameters.BitsToUse[1];
            Bit2 = _steganographyParameters.BitsToUse[2];
            Bit3 = _steganographyParameters.BitsToUse[3];
            Bit4 = _steganographyParameters.BitsToUse[4];
            Bit5 = _steganographyParameters.BitsToUse[5];
            Bit6 = _steganographyParameters.BitsToUse[6];
            Bit7 = _steganographyParameters.BitsToUse[7];
        }

        public SteganographyParametersVM(SteganographyParameters steganographyParameters)
        {
            _steganographyParameters = steganographyParameters;

            MessageLength = _steganographyParameters.MessageLength.ToString();
            SpaceToHide = _steganographyParameters.SpaceToHide.ToString();
            DecryptionDisplayLength = _steganographyParameters.DecryptionDisplayLength.ToString();
            SpaceToRead = _steganographyParameters.SpaceToRead.ToString();
            DimR = _steganographyParameters.DimToUse[0];
            DimG = _steganographyParameters.DimToUse[1];
            DimB = _steganographyParameters.DimToUse[2];
            Bit0 = _steganographyParameters.BitsToUse[0];
            Bit1 = _steganographyParameters.BitsToUse[1];
            Bit2 = _steganographyParameters.BitsToUse[2];
            Bit3 = _steganographyParameters.BitsToUse[3];
            Bit4 = _steganographyParameters.BitsToUse[4];
            Bit5 = _steganographyParameters.BitsToUse[5];
            Bit6 = _steganographyParameters.BitsToUse[6];
            Bit7 = _steganographyParameters.BitsToUse[7];
        }

        public void UpdateSpaceToHide(int messageLength, int spaceToHide)
        {
            MessageLength = messageLength.ToString();
            SpaceToHide = spaceToHide.ToString();
        }

        public void UpdateDecryptionDisplayLength(int decryptionDisplayLength)
        {
            DecryptionDisplayLength = decryptionDisplayLength.ToString();
        }

        public void UpdateSpaceToRead(int spaceToRead)
        {
            SpaceToRead = spaceToRead.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
