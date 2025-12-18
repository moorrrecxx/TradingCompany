using System.Windows;
using TradingCompany.WPF.ViewModels;

namespace TradingCompany.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}