using System.Windows;
using Kurs11.ViewModels;

namespace Kurs11.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = pwdBox.Password;
                vm.LoginCommand.Execute(null);
            }
        }
    }
}