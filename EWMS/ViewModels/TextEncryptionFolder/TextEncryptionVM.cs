using EWMS.Models.Containers;
using EWMS.Models.Converters;
using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EWMS.ViewModels
{
    internal class TextEncryptionVM : INotifyPropertyChanged
    {
        private DataEncryption _dataEncryption;
        bool _IsInputTextModified;

        public ICommand ChangeToClassicCiphersListCommand { get; set; }
        public ICommand ChangeToEncryptionCommand { get; set; }
        public ICommand ChangeToDecryptionCommand { get; set; }
        public ICommand LoadInputTextCommand { get; set; }
        public ICommand SaveInputTextCommand { get; set; }
        public ICommand SaveOutputTextCommand { get; set; }
        public ICommand CopyInputTextToClipboardCommand { get; set; }
        public ICommand CopyOutputTextToClipboardCommand { get; set; }
        public ICommand ChangeToTextParamPanelCommand { get; set; }
        public ICommand ChangeToCipherParamPanelCommand { get; set; }
        public ICommand ChangeToCipherDescriptionPanelCommand { get; set; }
        public ICommand DoEncryptionCommand { get; set; }


        private ObservableCollection<CipherTypeButton> _cipherTypeList;
        public ObservableCollection<CipherTypeButton> CipherTypeList
        {
            get => _cipherTypeList;
            set
            {
                _cipherTypeList = value;
                OnPropertyChanged();
            }
        }

        private string _cipherName;
        public string CipherName
        {
            get => _cipherName;
            set
            {
                _cipherName = value;
                OnPropertyChanged();
            }
        }

        private string _decryptionEncryptionHeader;
        public string DecryptionEncryptionHeader
        {
            get => _decryptionEncryptionHeader;
            set
            {
                _decryptionEncryptionHeader = value;
                OnPropertyChanged();
            }
        }

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set
            {
                string text;
                if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && (_dataEncryption.Cipher.ReturnCipherType() == CipherType.Nihilist || _dataEncryption.Cipher.ReturnCipherType() == CipherType.Polybius))
                {
                    text = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), 25000);
                }
                else
                {
                    text = ValidateCommonInput.ValidateTextMaxLength(ValidateToByteRange.ValidateTextToString(value), 10000);
                }


                if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                {
                    if (_IsInputTextModified == false)
                    {
                        if (!_dataEncryption.InputBinE.CheckIfDataIsModified())
                        {
                            _dataEncryption.InputBinE.DataOriginal = StringByteListConverter.StringToByteList(text);
                        }
                    }
                    else
                    {
                        _dataEncryption.InputBinE.OverriteDataModified();
                        _dataEncryption.InputBinE.ModifyText();
                    }
                    _inputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinE.DataModified);
                }
                else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                {
                    if (_IsInputTextModified == false)
                    {
                        if (!_dataEncryption.InputBinD.CheckIfDataIsModified())
                        {
                            _dataEncryption.InputBinD.DataOriginal = StringByteListConverter.StringToByteList(text);
                        }
                    }
                    else
                    {
                        _dataEncryption.InputBinD.OverriteDataModified();
                        _dataEncryption.InputBinD.ModifyText();
                    }
                    _inputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinD.DataModified);
                }


                OutputText = string.Empty;
                if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                {
                    _dataEncryption.OutputBinE = new();
                }
                else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                {
                    _dataEncryption.OutputBinD = new();
                }

                OnPropertyChanged();
            }
        }

        public List<byte> InputDataModifiedUpdated
        {
            get
            {
                if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                {
                    return _dataEncryption.InputBinE.DataModified;
                }
                else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                {
                    return _dataEncryption.InputBinD.DataModified;
                }
                return new();
            }
            set => _dataEncryption.InputBinD.DataModified = value;
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

        internal DataEncryption DataEncryption { get => _dataEncryption; set => _dataEncryption = value; }

        private object _textParametersVM;

        public object TextParametersVM
        {
            get => _textParametersVM;

            set { _textParametersVM = value; OnPropertyChanged(nameof(TextParametersVM)); }
        }

        private object _textParamDescriptionPanelVM;

        public object TextParamDescriptionPanelVM
        {
            get => _textParamDescriptionPanelVM;

            set { _textParamDescriptionPanelVM = value; OnPropertyChanged(nameof(TextParamDescriptionPanelVM)); }
        }

        public TextEncryptionVM()
        {
            _dataEncryption = new DataEncryption();
            _IsInputTextModified = false;
            InitializeCipherList(CipherClass.Classic);
            _dataEncryption.EnDe = EncryptDecrypt.Encryption;
            DecryptionEncryptionHeader = "ENCRYPTION";
            TextParametersVM = new TextParametersVM();
            CipherTypeUpdated(new CaesarCipher());
            ChangeToEncryptionCommand = new RelayCommand(ChangeToEncryption);
            ChangeToDecryptionCommand = new RelayCommand(ChangeToDecryption);
            LoadInputTextCommand = new RelayCommand(LoadInputText);
            SaveInputTextCommand = new RelayCommand(SaveInputText);
            SaveOutputTextCommand = new RelayCommand(SaveOutputText);
            CopyInputTextToClipboardCommand = new RelayCommand(CopyInputTextToClipboard);
            CopyOutputTextToClipboardCommand = new RelayCommand(CopyOutputTextToClipboard);
            ChangeToTextParamPanelCommand = new RelayCommand(ChangeToTextParamPanel);
            ChangeToCipherParamPanelCommand = new RelayCommand(ChangeToCipherParamPanel);
            ChangeToCipherDescriptionPanelCommand = new RelayCommand(ChangeToCipherDescriptionPanel);
            DoEncryptionCommand = new RelayCommand(DoEncryption);
        }

        private void ChangeToEncryption(object obj)
        {
            _dataEncryption.EnDe = EncryptDecrypt.Encryption;
            DecryptionEncryptionHeader = "ENCRYPTION";
            _dataEncryption.InputBinD = new();
            _dataEncryption.OutputBinD = new();

            TextParametersVM = new TextParametersVM();
            if (TextParamDescriptionPanelVM.GetType() == typeof(TextParametersVM))
            {
                TextParamDescriptionPanelVM = TextParametersVM;
                ((TextParametersVM)TextParamDescriptionPanelVM).ChildTextParametersUpdated += TextParametersUpdated;
                _dataEncryption.InputBinE.UpdateParam(new TextParameters());
            }

            InputText = string.Empty;
            OutputText = string.Empty;
        }

        private void ChangeToDecryption(object obj)
        {
            _dataEncryption.EnDe = EncryptDecrypt.Decryption;
            DecryptionEncryptionHeader = "DECRYPTION";
            _dataEncryption.InputBinD = new();
            _dataEncryption.OutputBinD = new();

            TextParametersVM = new TextParametersVM();
            if (TextParamDescriptionPanelVM.GetType() == typeof(TextParametersVM))
            {
                TextParamDescriptionPanelVM = TextParametersVM;
                ((TextParametersVM)TextParamDescriptionPanelVM).ChildTextParametersUpdated += TextParametersUpdated;
                _dataEncryption.InputBinD.UpdateParam(new TextParameters());
            }

            InputText = string.Empty;
            OutputText = string.Empty;
        }

        private void LoadInputText(object obj)
        {
            string path = DataFile.GetLoadTextPath();
            if (path != string.Empty)
            {
                if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                {
                    if (ValidateFileSize.ValidateSizeOfFile(path, 10000))
                    {
                        _dataEncryption.InputBinE.DataOriginal = DataFile.LoadText(path);

                        TextParametersVM = new TextParametersVM();
                        if (TextParamDescriptionPanelVM.GetType() == typeof(TextParametersVM))
                        {
                            TextParamDescriptionPanelVM = TextParametersVM;
                            ((TextParametersVM)TextParamDescriptionPanelVM).ChildTextParametersUpdated += TextParametersUpdated;
                            _dataEncryption.InputBinE.UpdateParam(new TextParameters());
                        }

                        InputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinE.DataOriginal);
                    }
                    else
                    {
                        DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The text you are trying to load is bigger than 10kB bytes!");
                    }
                }
                else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                {
                    if (_dataEncryption.Cipher.ReturnCipherType() == CipherType.Nihilist || _dataEncryption.Cipher.ReturnCipherType() == CipherType.Polybius)
                    {
                        if (ValidateFileSize.ValidateSizeOfFile(path, 25000))
                        {
                            _dataEncryption.InputBinD.DataOriginal = DataFile.LoadText(path);

                            TextParametersVM = new TextParametersVM();
                            if (TextParamDescriptionPanelVM.GetType() == typeof(TextParametersVM))
                            {
                                TextParamDescriptionPanelVM = TextParametersVM;
                                ((TextParametersVM)TextParamDescriptionPanelVM).ChildTextParametersUpdated += TextParametersUpdated;
                                _dataEncryption.InputBinD.UpdateParam(new TextParameters());
                            }

                            InputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinD.DataOriginal);
                        }
                        else
                        {
                            DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The text you are trying to load is bigger than 25kB bytes!");
                        }
                    }
                    else 
                    {
                        if (ValidateFileSize.ValidateSizeOfFile(path, 10000))
                        {
                            _dataEncryption.InputBinD.DataOriginal = DataFile.LoadText(path);
                            InputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinD.DataOriginal);
                        }
                        else
                        {
                            DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The text you are trying to load is bigger than 10kB bytes!");
                        }
                    }
                }
            }
        }

        private void SaveInputText(object obj)
        {
            string path;
            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption && _dataEncryption.InputBinE.DataModified.Count != 0)
            {
                path = DataFile.GetSaveTextPath();

                if (path != string.Empty)
                {
                    DataFile.WriteText(path, _dataEncryption.InputBinE.DataModified);
                }
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && _dataEncryption.InputBinD.DataModified.Count != 0)
            {
                path = DataFile.GetSaveTextPath();

                if (path != string.Empty)
                {
                    DataFile.WriteText(path, _dataEncryption.InputBinD.DataModified);
                }
            }
        }

        private void SaveOutputText(object obj)
        {
            string path;
            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption && _dataEncryption.OutputBinE.Count != 0)
            {
                path = DataFile.GetSaveTextPath();

                if (path != string.Empty)
                {
                    DataFile.WriteText(path, _dataEncryption.OutputBinE);

                }
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && _dataEncryption.OutputBinD.Count != 0)
            {
                path = DataFile.GetSaveTextPath();

                if (path != string.Empty)
                {
                    DataFile.WriteText(path, _dataEncryption.OutputBinD);
                }
            }
        }

        private void CopyInputTextToClipboard(object obj)
        {
            if (InputText != string.Empty)
            {
                Clipboard.SetText(InputText);
            }
        }

        private void CopyOutputTextToClipboard(object obj)
        {
            if (OutputText != string.Empty)
            {
                Clipboard.SetText(OutputText);
            }
        }

        private void ChangeToTextParamPanel(object obj)
        {
            TextParamDescriptionPanelVM = TextParametersVM;
            ((TextParametersVM)TextParamDescriptionPanelVM).ChildTextParametersUpdated += TextParametersUpdated;
        }

        private void ChangeToCipherParamPanel(object obj)
        {
            UpdateCipherParametersUserControl(_dataEncryption.Cipher.ReturnCipherType());
            OnPropertyChanged();
        }

        private void ChangeToCipherDescriptionPanel(object obj)
        {
            TextParamDescriptionPanelVM = new DescriptionVM(_dataEncryption.Cipher.ReturnCipherDescription());
        }

        private void DoEncryption(object obj)
        {
            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption && _dataEncryption.InputBinE.DataModified.Count != 0 && _dataEncryption.Cipher.CheckparamValidation() == string.Empty && _dataEncryption.Cipher.CheckparamValidation(_dataEncryption.InputBinE) == string.Empty)
            {
                _dataEncryption.Encryption();
                if (_dataEncryption.OutputBinE.Count != 0)
                {
                    OutputText = StringByteListConverter.ByteListToString(_dataEncryption.OutputBinE);
                }
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && _dataEncryption.InputBinD.DataModified.Count != 0 && _dataEncryption.Cipher.CheckparamValidation() == string.Empty && _dataEncryption.Cipher.CheckparamValidation(_dataEncryption.InputBinD) == string.Empty)
            {
                _dataEncryption.Encryption();
                if (_dataEncryption.OutputBinD.Count != 0)
                {
                    OutputText = StringByteListConverter.ByteListToString(_dataEncryption.OutputBinD);
                }
            }
            else
            {
                string message = string.Empty;

                if (_dataEncryption.EnDe == EncryptDecrypt.Encryption && _dataEncryption.InputBinE.DataModified.Count == 0)
                {
                    message += "Input data is empty!\n";
                }
                else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && _dataEncryption.InputBinD.DataModified.Count == 0)
                {
                    message += "Input data is empty!\n";
                }
                else
                {
                    if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                    {
                        message += _dataEncryption.Cipher.CheckparamValidation();
                        message += _dataEncryption.Cipher.CheckparamValidation(_dataEncryption.InputBinE);
                    }
                    else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                    {
                        message += _dataEncryption.Cipher.CheckparamValidation();
                        message += _dataEncryption.Cipher.CheckparamValidation(_dataEncryption.InputBinD);
                    }
                }

                if (message != string.Empty)
                {
                    DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, message);

                    OutputText = string.Empty;
                    if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                    {
                        _dataEncryption.OutputBinE = new();
                    }
                    else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                    {
                        _dataEncryption.OutputBinD = new();
                    }
                }
            }
        }

        private void InitializeCipherList(CipherClass cipherClass)
        {
            ICipher iCipher = null;

            CipherTypeList = new ObservableCollection<CipherTypeButton>();

            foreach (CipherType cipher in (CipherType[])Enum.GetValues(typeof(CipherType)))
            {
                if (cipher != CipherType.None)
                {
                    iCipher = Creator.GetCipher(cipher);
                    if (iCipher.ReturnCipherClass() == cipherClass)
                    {
                        CipherTypeButton c = new() { CipherTypeName = iCipher.ReturnCipherName().ToString(), CipherTypeEnum = cipher, CipherClassEnum = iCipher.ReturnCipherClass() };
                        c.ChildCipherUpdated += CipherTypeUpdated;
                        CipherTypeList.Add(c);
                    }
                }
            }
        }

        private void UpdateCipherParametersUserControl(CipherType cipherType)
        {
            if (cipherType == CipherType.RC4)
            {
                TextParamDescriptionPanelVM = new RC4ParamVM();
                ((RC4ParamVM)TextParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else if (cipherType == CipherType.Caesar)
            {
                TextParamDescriptionPanelVM = new CaesarParamVM();
                ((CaesarParamVM)TextParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else if (cipherType == CipherType.Fence)
            {
                TextParamDescriptionPanelVM = new FenceParamVM();
                ((FenceParamVM)TextParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else if (cipherType == CipherType.Nihilist)
            {
                TextParamDescriptionPanelVM = new NihilistParamVM();
                ((NihilistParamVM)TextParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else if (cipherType == CipherType.Vigenere)
            {
                TextParamDescriptionPanelVM = new VigenereParamVM();
                ((VigenereParamVM)TextParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else if (cipherType == CipherType.AES)
            {
                TextParamDescriptionPanelVM = new AESParamVM();
                ((AESParamVM)TextParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else
            {
                TextParamDescriptionPanelVM = new NoParamVM();
            }

            _dataEncryption.Cipher.ResetParam();
        }

        private void CipherTypeUpdated(ICipher cipher)
        {
            _dataEncryption = new();
            _dataEncryption.Cipher = cipher;
            _dataEncryption.EnDe = EncryptDecrypt.Encryption;
            DecryptionEncryptionHeader = "ENCRYPTION";

            InputText = string.Empty;
            OutputText = string.Empty;

            TextParametersVM = new TextParametersVM();
            TextParamDescriptionPanelVM = TextParametersVM;
            ((TextParametersVM)TextParamDescriptionPanelVM).ChildTextParametersUpdated += TextParametersUpdated;

            CipherName = _dataEncryption.Cipher.ReturnCipherName();
            UpdateCipherParametersUserControl(_dataEncryption.Cipher.ReturnCipherType());
        }

        private void CipherParamUpdated(object cipherParam)
        {
            _dataEncryption.Cipher.UpdateParam(cipherParam);
        }

        private void TextParametersUpdated(TextParameters textParameters)
        {
            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
            {
                _dataEncryption.InputBinE.UpdateParam(textParameters);
                _IsInputTextModified = true;
                InputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinE.DataOriginal);
                _IsInputTextModified = false;
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
            {
                _dataEncryption.InputBinD.UpdateParam(textParameters);
                _IsInputTextModified = true;
                InputText = StringByteListConverter.ByteListToString(_dataEncryption.InputBinD.DataOriginal);
                _IsInputTextModified = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
