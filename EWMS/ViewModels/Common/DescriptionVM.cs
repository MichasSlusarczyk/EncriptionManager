using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EWMS.ViewModels
{
    internal class DescriptionVM : INotifyPropertyChanged
    {
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public DescriptionVM()
        {
            Description = "No description...";
        }
        public DescriptionVM(string description)
        {
            Description = description;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
