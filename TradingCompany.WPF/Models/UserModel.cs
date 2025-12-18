using System.ComponentModel;
using System.Text.RegularExpressions;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;

namespace TradingCompany.WPF.Models
{
    public class UserModel : ViewModelBase, IDataErrorInfo
    {
        private int _userId;
        private string _login;
        private string _email;

        public int UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(); }
        }

        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case nameof(Login):
                        if (string.IsNullOrWhiteSpace(Login))
                            result = "Login cannot be empty.";
                        else if (Login.Length < 3)
                            result = "Login must be at least 3 characters.";
                        break;

                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Email cannot be empty.";
                        else if (!IsValidEmail(Email))
                            result = "Invalid email format.";
                        break;
                }
                return result;
            }
        }

        public bool IsValid => string.IsNullOrEmpty(this[nameof(Login)]) &&
                               string.IsNullOrEmpty(this[nameof(Email)]);

        public UserDTO ToDTO()
        {
            return new UserDTO
            {
                UserId = this.UserId,
                Login = this.Login,
                Email = this.Email
            };
        }
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}