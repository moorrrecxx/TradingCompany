using Microsoft.Extensions.DependencyInjection;
using System;
using TradingCompany.WPF.Core;

namespace TradingCompany.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ShowLogin();
        }

        private void ShowLogin()
        {
            var loginVm = _serviceProvider.GetRequiredService<LoginViewModel>();

            loginVm.LoginSuccess += (roleName) =>
            {
                if (roleName == "Admin")
                {
                    ShowAdminHome(); 
                }
                else
                {
                    ShowUserHome(); 
                }
            };

            CurrentView = loginVm;
        }

        private void ShowAdminHome()
        {
            var adminVm = _serviceProvider.GetRequiredService<AdminHomeViewModel>();
            adminVm.LogoutRequested += ShowLogin;
            CurrentView = adminVm;
        }

        private void ShowUserHome()
        {
            var userVm = _serviceProvider.GetRequiredService<UserHomeViewModel>();
            userVm.LogoutRequested += ShowLogin;
            CurrentView = userVm;
        }
    }
}