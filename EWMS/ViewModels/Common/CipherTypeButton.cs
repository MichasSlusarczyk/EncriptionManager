using EWMS.Models.EncryptionFolder;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EWMS.ViewModels
{
    internal class CipherTypeButton : INotifyPropertyChanged
    {
        public event Action<ICipher> ChildCipherUpdated;

        public ICommand ChangeCipherTypeCommand { get; set; }

        public CipherTypeButton()
        {
            ChangeCipherTypeCommand = new RelayCommand(ChangeCipherType);
        }

        private string _cipherTypeName;
        public string CipherTypeName
        {
            get => _cipherTypeName;
            set
            {
                _cipherTypeName = value;
                OnPropertyChanged();
            }
        }

        private CipherType _cipherTypeEnum;
        public CipherType CipherTypeEnum
        {
            get => _cipherTypeEnum;
            set => _cipherTypeEnum = value;
        }

        private CipherClass _cipherClassEnum;
        public CipherClass CipherClassEnum
        {
            get => _cipherClassEnum;
            set => _cipherClassEnum = value;
        }

        private void ChangeCipherType(object obj)
        {
            ICipher iCipher = Creator.GetCipher(_cipherTypeEnum);
            ChildCipherUpdated(iCipher);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
