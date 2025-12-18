using System.ComponentModel;
using TradingCompany.WPF.Core;

namespace TradingCompany.WPF.Models
{
    public class LoginModel : ViewModelBase, IDataErrorInfo
    {
        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == nameof(Login))
                {
                    if (string.IsNullOrWhiteSpace(Login))
                    {
                        result = "Login cannot be empty.";
                    }
                }
                return result;
            }
        }

        public bool IsValid => string.IsNullOrEmpty(this[nameof(Login)]);
    }
}