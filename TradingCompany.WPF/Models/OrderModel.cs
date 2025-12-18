using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TradingCompany.DTO;

namespace TradingCompany.WPF.Models
{
    public class OrderModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private int _orderId;
        private string _customerName;
        private string _address;
        private string _phone;
        private DateTime _createdAt;
        private int _statusId; 

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public int OrderId
        {
            get => _orderId;
            set { _orderId = value; OnPropertyChanged(); }
        }

        public string CustomerName
        {
            get => _customerName;
            set { _customerName = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(); }
        }

        public int StatusId
        {
            get => _statusId;
            set { _statusId = value; OnPropertyChanged(); }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(CustomerName):
                        if (string.IsNullOrWhiteSpace(CustomerName)) error = "Name is required.";
                        break;
                    case nameof(Phone):
                        if (string.IsNullOrWhiteSpace(Phone)) error = "Phone is required.";
                        break;
                    case nameof(StatusId):
                        if (StatusId <= 0) error = "Select a status.";
                        break;
                }
                return error;
            }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(CustomerName) &&
                               !string.IsNullOrWhiteSpace(Phone) &&
                               StatusId > 0;

        public static OrderModel FromDTO(OrderDTO dto)
        {
            return new OrderModel
            {
                OrderId = dto.OrderId,
                CustomerName = dto.CustomerName,
                Address = dto.Address,
                Phone = dto.Phone,
                CreatedAt = dto.CreatedAt,
                StatusId = dto.Status?.StatusId ?? 0
            };
        }

        public OrderDTO ToDTO()
        {
            return new OrderDTO
            {
                OrderId = this.OrderId,
                CustomerName = this.CustomerName,
                Address = this.Address,
                Phone = this.Phone,
                CreatedAt = this.CreatedAt,
                Status = new StatusDTO { StatusId = this.StatusId }
            };
        }
    }
}