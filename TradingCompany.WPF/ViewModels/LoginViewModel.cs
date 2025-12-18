using System;
using System.Windows.Controls;
using System.Windows.Input;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Models; 
using TradingCompany.WPF.Services.Interfaces;

namespace TradingCompany.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthentication _authService;
        private string _errorMessage;

        public LoginModel LoginData { get; set; } = new LoginModel();

        public event Action<string> LoginSuccess;

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthentication authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            ErrorMessage = string.Empty;

            if (!LoginData.IsValid)
            {
                ErrorMessage = "Please enter a valid login.";
                return;
            }


            var passwordBox = parameter as PasswordBox;
            var password = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Please enter your password.";
                return;
            }

            try
            {
               
                if (_authService.ValidateUser(LoginData.Login, password, out string role))
                {
                    LoginSuccess?.Invoke(role);
                }
                else
                {
                    ErrorMessage = "Invalid login or password.";
                    passwordBox.Password = string.Empty; 
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"System Error: {ex.Message}";
            }
        }
    }
}