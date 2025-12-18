using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TradingCompany.DTO;

namespace TradingCompany.WPF.Models
{
    public class LogModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private int _logId;
        private string _action;
        private DateTime _createdAt;
        private int _userId;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public int LogId
        {
            get => _logId;
            set { _logId = value; OnPropertyChanged(); }
        }

        public string Action
        {
            get => _action;
            set { _action = value; OnPropertyChanged(); }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(); }
        }

        public int UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(); }
        }


        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(UserId):
                        if (UserId <= 0)
                            error = "Please select a user.";
                        break;

                    case nameof(Action):
                        if (string.IsNullOrWhiteSpace(Action))
                            error = "Action cannot be empty.";
                        break;
                }

                return error;
            }
        }
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(this[nameof(UserId)]) &&
                       string.IsNullOrEmpty(this[nameof(Action)]);
            }
        }
        public static LogModel FromDTO(LogDTO dto)
        {
            return new LogModel
            {
                LogId = dto.LogId,
                Action = dto.Action,
                CreatedAt = dto.CreatedAt,
                UserId = dto.User?.UserId ?? 0
            };
        }

        public LogDTO ToDTO()
        {
            return new LogDTO
            {
                LogId = this.LogId,
                Action = this.Action,
                CreatedAt = this.CreatedAt,
                User = new UserDTO { UserId = this.UserId }
            };
        }
    }
}