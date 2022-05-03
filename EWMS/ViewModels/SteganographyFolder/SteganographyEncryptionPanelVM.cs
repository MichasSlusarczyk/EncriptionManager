using EWMS.Models.Containers;
using EWMS.Models.Converters;
using EWMS.Models.SteganographyFolder;
using EWMS.Models.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EWMS.ViewModels
{
    internal class SteganographyEncryptionPanelVM : INotifyPropertyChanged
    {
        public event Action<Img> ChildInputImgUpdated;
        public event Action<Data> ChildInputTextUpdated;

        private Steganography _steganography;

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), 100000);
                OutputImage = new();
                _steganography.OutputImgE = new();
                _steganography.MessageDataE.DataModified = StringByteListConverter.StringToByteList(_inputText);
                ChildInputTextUpdated?.Invoke(_steganography.MessageDataE);
                OnPropertyChanged();
            }
        }

        private BitmapImage _inputImage;
        public BitmapImage InputImage
        {
            get => _inputImage;
            set
            {
                _inputImage = value;
                OutputImage = new();
                _steganography.OutputImgE = new();
                OnPropertyChanged();
            }
        }

        private BitmapImage _outputImage;
        public BitmapImage OutputImage
        {
            get => _outputImage;
            set
            {
                _outputImage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadInputTextCommand { get; set; }
        public ICommand SaveInputTextCommand { get; set; }
        public ICommand LoadInputImageCommand { get; set; }
        public ICommand SaveOutputImageCommand { get; set; }
        public ICommand CopyInputTextToCilipboardCommand { get; set; }

        internal Steganography Steganography
        {
            get => _steganography;
            set
            {
                _steganography = value;
                OutputImage = BitmapImageByteArrayConverter.ByteArrayToBitmapImage(BitmapByteArrayConverter.BitmapToByteArray(_steganography.OutputImgE.ImgToBitmap()));
            }
        }
        public SteganographyEncryptionPanelVM()
        {
            _steganography = new Steganography();
            InputText = string.Empty;
            InputImage = new();
            OutputImage = new();
            LoadInputTextCommand = new RelayCommand(LoadInputText);
            SaveInputTextCommand = new RelayCommand(SaveInputText);
            LoadInputImageCommand = new RelayCommand(LoadInputImage);
            SaveOutputImageCommand = new RelayCommand(SaveOutputImage);
            CopyInputTextToCilipboardCommand = new RelayCommand(CopyInputTextToCilipboard);
        }

        public SteganographyEncryptionPanelVM(Steganography steganography)
        {
            _steganography = steganography;
            InputText = string.Empty;
            InputImage = new();
            OutputImage = new();
            LoadInputTextCommand = new RelayCommand(LoadInputText);
            SaveInputTextCommand = new RelayCommand(SaveInputText);
            LoadInputImageCommand = new RelayCommand(LoadInputImage);
            SaveOutputImageCommand = new RelayCommand(SaveOutputImage);
            CopyInputTextToCilipboardCommand = new RelayCommand(CopyInputTextToCilipboard);
        }

        private void LoadInputText(object obj)
        {
            string path = DataFile.GetLoadTextPath();
            if (path != string.Empty)
            {
                if (ValidateFileSize.ValidateSizeOfFile(path, 100000))
                {
                    _steganography.MessageDataE.DataModified = DataFile.LoadText(path);
                    InputText = StringByteListConverter.ByteListToString(_steganography.MessageDataE.DataModified);
                    ChildInputTextUpdated(_steganography.MessageDataE);
                }
                else
                {
                    DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The text you are trying to load is bigger than 100kB bytes!");
                }
            }
        }

        private void SaveInputText(object obj)
        {
            if (_steganography.MessageDataE.DataModified.Count != 0)
            {
                string path = DataFile.GetSaveTextPath();
                if (path != string.Empty)
                {
                    DataFile.WriteText(path, _steganography.MessageDataE.DataModified);
                }
            }
        }

        private void LoadInputImage(object obj)
        {
            string path = DataFile.GetLoadImagePath();
            if (path != string.Empty)
            {
                if (ValidateFileSize.ValidateSizeOfFile(path, 1000000))
                {
                    _steganography.InputImgE = new(BitmapByteListConverter.ByteListToBitmap(DataFile.LoadFile(path)));
                    InputImage = BitmapImageByteArrayConverter.ByteArrayToBitmapImage(BitmapByteArrayConverter.BitmapToByteArray(_steganography.InputImgE.ImgToBitmap()));
                    ChildInputImgUpdated(_steganography.InputImgE);
                }
                else
                {
                    DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The image you are trying to load is bigger than 1 Mpx!");
                }
            }
        }

        private void SaveOutputImage(object obj)
        {
            if (_steganography.OutputImgE.Size != 0)
            {
                string path = DataFile.GetSaveImagePath();
                if (path != string.Empty)
                {
                    DataFile.WriteFile(path, BitmapByteListConverter.BitmapToByteList(_steganography.OutputImgE.ImgToBitmap()));
                }
            }
        }

        private void CopyInputTextToCilipboard(object obj)
        {
            if (InputText != string.Empty)
            {
                Clipboard.SetText(InputText);
            }
        }

        public void UpdateSteganography(Steganography steganography)
        {
            Steganography = steganography;
        }

        public void ResetOutputImage()
        {
            OutputImage = new();
            Steganography.OutputImgE = new();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
