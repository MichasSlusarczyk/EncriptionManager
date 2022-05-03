using EWMS.ViewModels;
using System.Windows;

namespace EWMS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM _mainWindowVM = new();
            DataContext = _mainWindowVM;
            Closing += _mainWindowVM.OnWindowClosing;
        }
    }
}
