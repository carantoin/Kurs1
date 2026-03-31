using System.Windows;
using Kurs11.ViewModels;

namespace Kurs11.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string token)
        {
            InitializeComponent();
            DataContext = new NavigationViewModel(token);
        }
    }
}