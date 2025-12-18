using System;
using System.Windows;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Models; 
namespace TradingCompany.WPF.ViewModels
{
    public class UserEditorViewModel : ViewModelBase
    {
        private readonly IUserDAL _userDal;

        public UserModel User { get; set; }

        public string Password { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseRequest;

        public UserEditorViewModel(IUserDAL userDal)
        {
            _userDal = userDal;
            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(obj => CloseRequest?.Invoke());
        }

        public void SetUser(UserDTO userDto)
        {
            User = new UserModel
            {
                UserId = userDto.UserId,
                Login = userDto.Login,
                Email = userDto.Email
            };
            OnPropertyChanged(nameof(User));
        }

        private void ExecuteSave(object obj)
        {
            if (!User.IsValid)
            {
                MessageBox.Show("Please correct the errors in the form (Login or Email format).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (User.UserId == 0 && string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Password is required for new users.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsLoginTaken(User.Login, User.UserId))
                {
                    MessageBox.Show($"Login '{User.Login}' is already taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (IsEmailTaken(User.Email, User.UserId))
                {
                    MessageBox.Show($"Email '{User.Email}' is already registered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var dto = User.ToDTO();

                if (dto.UserId == 0)
                {
                    _userDal.Create(dto, Password);
                }
                else
                {
                    _userDal.Update(dto);
                }

                CloseRequest?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool IsLoginTaken(string login, int currentUserId)
        {
            var user = _userDal.GetUserByLogin(login);
            return user != null && user.UserId != currentUserId;
        }
        private bool IsEmailTaken(string email, int currentUserId)
        {
            var user = _userDal.GetUserByEmail(email);
            return user != null && user.UserId != currentUserId;
        }
    }
}