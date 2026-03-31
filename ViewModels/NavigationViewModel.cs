using System.Windows.Input;
using Kurs11.Utils;

namespace Kurs11.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly string _token;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; }
        public ICommand ClientsCommand { get; }
        public ICommand ContractsCommand { get; }

        public NavigationViewModel(string token)
        {
            _token = token;

            HomeCommand = new RelayCommand(o => CurrentView = new HomeViewModel());

            CurrentView = new HomeViewModel();
        }
    }
}