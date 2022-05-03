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
    internal class SteganographyDecryptionPanelVM : INotifyPropertyChanged
    {
        public event Action<Img> ChildInputImgUpdated;

        private Steganography _steganography;

        private BitmapImage _inputImage;
        public BitmapImage InputImage
        {
            get => _inputImage;
            set
            {
                _inputImage = value;
                OutputText = string.Empty;
                _steganography.MessageDataD = new();
                OnPropertyChanged();
            }
        }

        private string _outputText;
        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadImageCommand { get; set; }
        public ICommand SaveTextCommand { get; set; }
        public ICommand CopyOutputTextToCilipboardCommand { get; set; }
        internal Steganography Steganography
        {
            get => _steganography;
            set
            {
                _steganography = value;

                if (_steganography.CheckIfTheRevealDataFit())
                {
                    OutputText = StringByteListConverter.ByteListToString(_steganography.MessageDataD.DataModified).Substring(0, _steganography.Param.DecryptionDisplayLength);
                }
            }
        }

        public SteganographyDecryptionPanelVM()
        {
            _steganography = new Steganography();
            InputImage = new();
            OutputText = string.Empty;
            LoadImageCommand = new RelayCommand(LoadImage);
            SaveTextCommand = new RelayCommand(SaveText);
            CopyOutputTextToCilipboardCommand = new RelayCommand(CopyOutputTextToCilipboard);
        }

        public SteganographyDecryptionPanelVM(Steganography steganography)
        {
            _steganography = steganography;
            InputImage = new();
            OutputText = string.Empty;
            LoadImageCommand = new RelayCommand(LoadImage);
            SaveTextCommand = new RelayCommand(SaveText);
            CopyOutputTextToCilipboardCommand = new RelayCommand(CopyOutputTextToCilipboard);
        }

        private void LoadImage(object obj)
        {
            string path = DataFile.GetLoadImagePath();
            if (path != string.Empty)
            {
                if (ValidateFileSize.ValidateSizeOfFile(path, 1000000))
                {
                    _steganography.InputImgD = new(BitmapByteListConverter.ByteListToBitmap(DataFile.LoadFile(path)));
                    InputImage = BitmapImageByteArrayConverter.ByteArrayToBitmapImage(BitmapByteArrayConverter.BitmapToByteArray(_steganography.InputImgD.ImgToBitmap()));
                    ChildInputImgUpdated(_steganography.InputImgD);
                }
                else
                {
                    DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The image you are trying to load is bigger than 1 Mpx!");
                }
            }
        }

        private void SaveText(object obj)
        {
            if (_steganography.MessageDataD.DataModified.Count != 0)
            {
                string path = DataFile.GetSaveTextPath();
                if (path != string.Empty)
                {
                    DataFile.WriteText(path, _steganography.MessageDataD.DataModified);
                }
            }
        }

        private void CopyOutputTextToCilipboard(object obj)
        {
            if (OutputText != string.Empty)
            {
                Clipboard.SetText(OutputText);
            }
        }

        public void UpdateSteganography(Steganography steganography)
        {
            Steganography = steganography;
        }

        public void ResetOutputText()
        {
            OutputText = string.Empty;
            Steganography.MessageDataD = new();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
