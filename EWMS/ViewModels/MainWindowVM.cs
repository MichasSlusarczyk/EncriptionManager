using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EWMS.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        public ICommand GoToPasswordManagerUCCommand { get; set; }
        public ICommand GoToTextEncryptionUCCommand { get; set; }
        public ICommand GoToFileEncryptionUCCommand { get; set; }
        public ICommand GoToSteganographyUCCommand { get; set; }

        private object _mainWindowFuctionalityVM;

        public object MainWindowFuctionalityVM

        {

            get => _mainWindowFuctionalityVM;

            set { _mainWindowFuctionalityVM = value; OnPropertyChanged(nameof(MainWindowFuctionalityVM)); }

        }
        public MainWindowVM()
        {
            MainWindowFuctionalityVM = new TextEncryptionVM();

            GoToPasswordManagerUCCommand = new RelayCommand(GoToPasswordManagerUC);
            GoToSteganographyUCCommand = new RelayCommand(GoToSteganographyUC);
            GoToFileEncryptionUCCommand = new RelayCommand(GoToFileEncryptionUC);
            GoToTextEncryptionUCCommand = new RelayCommand(GoToTextncriptionUC);
        }


        private void GoToPasswordManagerUC(object obj)
        {
            MainWindowFuctionalityVM = new PasswordManagerVM();
        }
        private void GoToSteganographyUC(object obj)
        {
            MainWindowFuctionalityVM = new SteganographyVM();
        }

        private void GoToFileEncryptionUC(object obj)
        {
            MainWindowFuctionalityVM = new FileEncryptionVM();
        }

        private void GoToTextncriptionUC(object obj)
        {
            MainWindowFuctionalityVM = new TextEncryptionVM();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            MainWindowFuctionalityVM = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
