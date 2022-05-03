using EWMS.Models.Containers;
using EWMS.Models.SteganographyFolder;
using EWMS.Models.Validation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EWMS.ViewModels
{
    internal class SteganographyVM : INotifyPropertyChanged
    {
        private readonly Steganography _steganography;

        public ICommand GoToEncryptionPanelCommand { get; set; }
        public ICommand GoToDecryptionPanelCommand { get; set; }
        public ICommand GoToSteganographyParametersCommand { get; set; }
        public ICommand GoToSteganographyDescriptionCommand { get; set; }
        public ICommand DoSteganographyCommand { get; set; }

        private object _enriptionDecryptionSteganographyPanelVM;

        private object _descriptionParametersPanelVM;

        public object EnriptionDecryptionSteganographyPanelVM
        {
            get => _enriptionDecryptionSteganographyPanelVM;

            set { _enriptionDecryptionSteganographyPanelVM = value; OnPropertyChanged(nameof(EnriptionDecryptionSteganographyPanelVM)); }
        }

        public object DescriptionParametersPanelVM

        {
            get => _descriptionParametersPanelVM;

            set { _descriptionParametersPanelVM = value; OnPropertyChanged(nameof(DescriptionParametersPanelVM)); }
        }

        public SteganographyVM()
        {
            _steganography = new Steganography
            {
                EnDe = EncryptDecrypt.Encryption
            };
            EnriptionDecryptionSteganographyPanelVM = new SteganographyEncryptionPanelVM(_steganography);
            ((SteganographyEncryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).ChildInputTextUpdated += SteganographyMessageDataEUpdated;
            object enriptionDecryptionSteganographyPanelVM = EnriptionDecryptionSteganographyPanelVM;
            ((SteganographyEncryptionPanelVM)enriptionDecryptionSteganographyPanelVM).ChildInputImgUpdated += SteganographyInputImgEUpdated;
            DescriptionParametersPanelVM = new SteganographyParametersVM(_steganography.Param);
            ((SteganographyParametersVM)DescriptionParametersPanelVM).ChildSteganographyParametersUpdated += SteganographyParametersUpdated;

            GoToEncryptionPanelCommand = new RelayCommand(GoToEncryptionPanelUC);
            GoToDecryptionPanelCommand = new RelayCommand(GoToDecryptionPanelUC);
            GoToSteganographyParametersCommand = new RelayCommand(GoToSteganographyParametersUC);
            GoToSteganographyDescriptionCommand = new RelayCommand(GoToSteganographyDescriptionUC);
            DoSteganographyCommand = new RelayCommand(DoSteganography);
        }

        private void GoToEncryptionPanelUC(object obj)
        {
            _steganography.EnDe = EncryptDecrypt.Encryption;
            _steganography.MessageDataD = new();
            _steganography.InputImgD = new();
            _steganography.Param.DecryptionDisplayLength = 0;
            EnriptionDecryptionSteganographyPanelVM = new SteganographyEncryptionPanelVM(_steganography);
            ((SteganographyEncryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).ChildInputTextUpdated += SteganographyMessageDataEUpdated;
            ((SteganographyEncryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).ChildInputImgUpdated += SteganographyInputImgEUpdated;

            if (DescriptionParametersPanelVM.GetType() == typeof(SteganographyParametersVM))
            {
                _steganography.CalculatePossibleSpaceToRead();
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToRead(_steganography.Param.SpaceToRead);
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateDecryptionDisplayLength(_steganography.Param.DecryptionDisplayLength);
            }

        }

        private void GoToDecryptionPanelUC(object obj)
        {
            _steganography.EnDe = EncryptDecrypt.Decryption;
            _steganography.OutputImgE = new();
            _steganography.InputImgE = new();
            _steganography.MessageDataE = new();
            _steganography.Param.DecryptionDisplayLength = 0;
            EnriptionDecryptionSteganographyPanelVM = new SteganographyDecryptionPanelVM(_steganography);
            ((SteganographyDecryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).ChildInputImgUpdated += SteganographyInputImgDUpdated;
            if (DescriptionParametersPanelVM.GetType() == typeof(SteganographyParametersVM))
            {
                _steganography.CalculatePossibleSpaceToHide();
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToHide(_steganography.Param.MessageLength, _steganography.Param.SpaceToHide);
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateDecryptionDisplayLength(_steganography.Param.DecryptionDisplayLength);
            }
        }

        private void GoToSteganographyParametersUC(object obj)
        {
            DescriptionParametersPanelVM = new SteganographyParametersVM(_steganography.Param);
            ((SteganographyParametersVM)DescriptionParametersPanelVM).ChildSteganographyParametersUpdated += SteganographyParametersUpdated;
            if (EnriptionDecryptionSteganographyPanelVM.GetType() == typeof(SteganographyEncryptionPanelVM))
            {
                _steganography.CalculatePossibleSpaceToHide();
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToHide(_steganography.Param.MessageLength, _steganography.Param.SpaceToHide);
            }
            else if (EnriptionDecryptionSteganographyPanelVM.GetType() == typeof(SteganographyDecryptionPanelVM))
            {
                _steganography.CalculatePossibleSpaceToRead();
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToRead(_steganography.Param.SpaceToRead);
            }

        }

        private void GoToSteganographyDescriptionUC(object obj)
        {
            DescriptionParametersPanelVM = new DescriptionVM(_steganography.Description);
        }

        private void DoSteganography(object obj)
        {
            if (_steganography.EnDe == EncryptDecrypt.Encryption)
            {
                if (_steganography.InputImgE.Size != 0 && _steganography.MessageDataE.DataModified.Count != 0 && _steganography.CheckIfTheHideDataFit())
                {
                    _steganography.Concealment();
                    ((SteganographyEncryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).UpdateSteganography(_steganography);
                }
                else
                {
                    ((SteganographyEncryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).ResetOutputImage();
                    DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, ValidateSteganography.ValidateDoSteganographyEncryption(_steganography));
                }
            }
            else if (_steganography.EnDe == EncryptDecrypt.Decryption)
            {
                if (_steganography.InputImgD.Size != 0 && _steganography.CheckIfTheRevealDataFit())
                {
                    _steganography.Concealment();
                    ((SteganographyDecryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).UpdateSteganography(_steganography);
                }
                else
                {
                    ((SteganographyDecryptionPanelVM)EnriptionDecryptionSteganographyPanelVM).ResetOutputText();
                    DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, ValidateSteganography.ValidateDoSteganographyDecryption(_steganography));
                    _steganography.MessageDataD = new();
                    _steganography.OutputImgE = new();
                }

            }
        }

        private void SteganographyMessageDataEUpdated(Data messageDataE)
        {
            _steganography.MessageDataE = messageDataE;
            _steganography.CalculatePossibleSpaceToHide();
            if (DescriptionParametersPanelVM.GetType() == typeof(SteganographyParametersVM))
            {
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToHide(_steganography.Param.MessageLength, _steganography.Param.SpaceToHide);
            }
        }

        private void SteganographyInputImgEUpdated(Img inputImgE)
        {
            _steganography.InputImgE = inputImgE;
            _steganography.CalculatePossibleSpaceToHide();
            if (DescriptionParametersPanelVM.GetType() == typeof(SteganographyParametersVM))
            {
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToHide(_steganography.Param.MessageLength, _steganography.Param.SpaceToHide);
            }
        }

        private void SteganographyInputImgDUpdated(Img inputImgD)
        {
            _steganography.InputImgD = inputImgD;
            _steganography.CalculatePossibleSpaceToRead();
            _steganography.Param.DecryptionDisplayLength = 0;
            if (DescriptionParametersPanelVM.GetType() == typeof(SteganographyParametersVM))
            {
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToRead(_steganography.Param.SpaceToRead);
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateDecryptionDisplayLength(_steganography.Param.DecryptionDisplayLength);
            }
        }

        private void SteganographyParametersUpdated(SteganographyParameters steganographyParameters)
        {
            _steganography.Param = steganographyParameters;
            _steganography.CalculatePossibleSpaceToHide();
            _steganography.CalculatePossibleSpaceToRead();
            if (DescriptionParametersPanelVM.GetType() == typeof(SteganographyParametersVM))
            {
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToHide(_steganography.Param.MessageLength, _steganography.Param.SpaceToHide);
                ((SteganographyParametersVM)DescriptionParametersPanelVM).UpdateSpaceToRead(_steganography.Param.SpaceToRead);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
