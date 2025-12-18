using System.ComponentModel;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;

namespace TradingCompany.WPF.Models
{
    public class StatusModel : ViewModelBase, IDataErrorInfo
    {
        private int _statusId;
        private string _name;

        public int StatusId
        {
            get => _statusId;
            set { _statusId = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == nameof(Name))
                {
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        result = "Status name cannot be empty.";
                    }
                }
                return result;
            }
        }

        public bool IsValid => string.IsNullOrEmpty(this[nameof(Name)]);

        public StatusDTO ToDTO()
        {
            return new StatusDTO
            {
                StatusId = this.StatusId,
                StatusName = this.Name 
            };
        }
    }
}