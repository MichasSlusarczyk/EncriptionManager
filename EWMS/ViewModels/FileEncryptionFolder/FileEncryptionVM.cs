using EWMS.Models.Containers;
using EWMS.Models.EncryptionFolder;
using EWMS.Models.Validation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EWMS.ViewModels
{
    internal class FileEncryptionVM : INotifyPropertyChanged
    {
        private DataEncryption _dataEncryption;
        public ICommand ChangeToStreamCiphersListCommand { get; set; }
        public ICommand ChangeToBlockCiphersListCommand { get; set; }
        public ICommand ChangeToEncryptionCommand { get; set; }
        public ICommand ChangeToDecryptionCommand { get; set; }
        public ICommand LoadInputFileCommand { get; set; }
        public ICommand SaveOutputFileCommand { get; set; }
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

        private string _inputFile;
        public string InputFile
        {
            get => _inputFile;
            set
            {
                _inputFile = value;
                OutputFile = string.Empty;
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

        private string _outputFile;
        public string OutputFile
        {
            get => _outputFile;
            set
            {
                _outputFile = value;
                OnPropertyChanged();
            }
        }

        internal DataEncryption DataEncryption { get => _dataEncryption; set => _dataEncryption = value; }


        private object _cipherParamDescriptionPanelVM;

        public object CipherParamDescriptionPanelVM
        {
            get => _cipherParamDescriptionPanelVM;

            set { _cipherParamDescriptionPanelVM = value; OnPropertyChanged(nameof(CipherParamDescriptionPanelVM)); }
        }

        public FileEncryptionVM()
        {
            _dataEncryption = new DataEncryption();
            InitializeCipherList(CipherClass.Stream);
            _dataEncryption.EnDe = EncryptDecrypt.Encryption;
            DecryptionEncryptionHeader = "ENCRYPTION";
            CipherTypeUpdated(new RC4Cipher());
            UpdateCipherParametersUserControl(_dataEncryption.Cipher.ReturnCipherType());
            ChangeToStreamCiphersListCommand = new RelayCommand(ChangeToStreamCiphersList);
            ChangeToBlockCiphersListCommand = new RelayCommand(ChangeToBlockCiphersList);
            ChangeToEncryptionCommand = new RelayCommand(ChangeToEncryption);
            ChangeToDecryptionCommand = new RelayCommand(ChangeToDecryption);
            LoadInputFileCommand = new RelayCommand(LoadInputFile);
            SaveOutputFileCommand = new RelayCommand(SaveOutputFile);
            ChangeToCipherDescriptionPanelCommand = new RelayCommand(ChangeToCipherDescriptionPanel);
            ChangeToCipherParamPanelCommand = new RelayCommand(ChangeToCipherParamPanel);
            DoEncryptionCommand = new RelayCommand(DoEncryption);
        }

        private void ChangeToStreamCiphersList(object obj)
        {
            InitializeCipherList(CipherClass.Stream);
        }

        private void ChangeToBlockCiphersList(object obj)
        {
            InitializeCipherList(CipherClass.Block);
        }

        private void ChangeToEncryption(object obj)
        {
            _dataEncryption.EnDe = EncryptDecrypt.Encryption;
            DecryptionEncryptionHeader = "ENCRYPTION";
            _dataEncryption.InputBinD = new();
            _dataEncryption.OutputBinD = new();
            InputFile = string.Empty;
            OutputFile = string.Empty;
        }

        private void ChangeToDecryption(object obj)
        {
            _dataEncryption.EnDe = EncryptDecrypt.Decryption;
            DecryptionEncryptionHeader = "DECRYPTION";
            _dataEncryption.InputBinE = new();
            _dataEncryption.OutputBinE = new();
            InputFile = string.Empty;
            OutputFile = string.Empty;

        }

        private void LoadInputFile(object obj)
        {

            string path = string.Empty;

            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
            {
                path = DataFile.GetLoadFilePathEncritpion();
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
            {
                path = DataFile.GetLoadFilePathDecritpion();
            }

            if (path != string.Empty)
            {
                if (_dataEncryption.EnDe == EncryptDecrypt.Encryption)
                {
                    if (ValidateFileSize.ValidateSizeOfFile(path, 100000))
                    {
                        _dataEncryption.InputBinE.DataModified = DataFile.LoadFileForEncryption(path);
                        InputFile = DataFile.GetInformationAboutFileToPrint(path);
                    }
                    else
                    {
                        DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The file you are trying to load is bigger than 100kB!");
                    }
                }
                else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption)
                {
                    if (ValidateFileSize.ValidateSizeOfFile(path, 101000))
                    {
                        _dataEncryption.InputBinD.DataModified = DataFile.LoadFileForDecryption(path);
                        InputFile = DataFile.InfoWhereLoadedDecryptionFileWas(path);
                    }
                    else
                    {
                        DisplayMessageBox.DisplayMessage(Application.Current.MainWindow, "The file you are trying to load is bigger than 100kB!");
                    }
                }
            }

        }

        private void SaveOutputFile(object obj)
        {
            string path;
            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption && _dataEncryption.OutputBinE.Count != 0)
            {
                path = DataFile.GetSaveFilePathEncryption();

                if (path != string.Empty)
                {
                    DataFile.WriteFileForEncryption(path, _dataEncryption.OutputBinE);
                    OutputFile += DataFile.InfoWhereFileWasSavedAfterEncryption(path);
                }
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && _dataEncryption.OutputBinD.Count != 0)
            {
                path = DataFile.GetSaveFilePathDecryption(DataFile.GetDecryptedFileExtention(_dataEncryption.OutputBinD));

                if (path != string.Empty)
                {
                    DataFile.WriteFileForDecryption(path, DataFile.ExtractDataFromDecryptedFile(_dataEncryption.OutputBinD));
                    OutputFile += DataFile.InfoWhereFileWasSavedAfterDecryption(path);
                }
            }
        }

        private void ChangeToCipherDescriptionPanel(object obj)
        {
            CipherParamDescriptionPanelVM = new DescriptionVM(_dataEncryption.Cipher.ReturnCipherDescription());
        }

        private void ChangeToCipherParamPanel(object obj)
        {
            UpdateCipherParametersUserControl(_dataEncryption.Cipher.ReturnCipherType());
        }


        private void DoEncryption(object obj)
        {
            if (_dataEncryption.EnDe == EncryptDecrypt.Encryption && _dataEncryption.InputBinE.DataModified.Count != 0 && _dataEncryption.Cipher.CheckparamValidation() == string.Empty && _dataEncryption.Cipher.CheckparamValidation(_dataEncryption.InputBinE) == string.Empty)
            {
                _dataEncryption.Encryption();
                if (_dataEncryption.OutputBinE.Count != 0)
                {
                    OutputFile = "File encrypted!\n\n";
                }
            }
            else if (_dataEncryption.EnDe == EncryptDecrypt.Decryption && _dataEncryption.InputBinD.DataModified.Count != 0 && _dataEncryption.Cipher.CheckparamValidation() == string.Empty && _dataEncryption.Cipher.CheckparamValidation(_dataEncryption.InputBinD) == string.Empty)
            {
                _dataEncryption.Encryption();
                if (_dataEncryption.OutputBinD.Count != 0)
                {
                    if (DataFile.CheckDecryptionQuality(_dataEncryption.OutputBinD))
                    {
                        OutputFile = "File decrypted successfully!\n\n" + DataFile.GetInformationAboutFileFromDecryption(_dataEncryption.OutputBinD);
                    }
                    else
                    {
                        OutputFile = "File decrypted unsuccessfully. Try again! \n\n";
                        _dataEncryption.OutputBinD.Clear();
                    }
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

                    OutputFile = string.Empty;
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
                CipherParamDescriptionPanelVM = new RC4ParamVM();
                ((RC4ParamVM)CipherParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else if (cipherType == CipherType.AES)
            {
                CipherParamDescriptionPanelVM = new AESParamVM();
                ((AESParamVM)CipherParamDescriptionPanelVM).ChildCipherParametersUpdated += CipherParamUpdated;
            }
            else
            {
                CipherParamDescriptionPanelVM = new NoParamVM();
            }
        }

        private void CipherTypeUpdated(ICipher cipher)
        {
            _dataEncryption = new();
            _dataEncryption.Cipher = cipher;
            _dataEncryption.EnDe = EncryptDecrypt.Encryption;
            DecryptionEncryptionHeader = "ENCRYPTION";

            InputFile = string.Empty;
            OutputFile = string.Empty;

            CipherName = _dataEncryption.Cipher.ReturnCipherName();
            UpdateCipherParametersUserControl(_dataEncryption.Cipher.ReturnCipherType());
        }

        private void CipherParamUpdated(object cipherParam)
        {
            _dataEncryption.Cipher.UpdateParam(cipherParam);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
