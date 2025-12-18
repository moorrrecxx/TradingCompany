using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TradingCompany.DTO;

namespace TradingCompany.WPF.Models
{
    public class ShipmentModel : INotifyPropertyChanged
    {
        private int _shipmentId;
        private int _orderId;
        private bool _confirmed;
        private DateTime _createdAt;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public int ShipmentId
        {
            get => _shipmentId;
            set { _shipmentId = value; OnPropertyChanged(); }
        }

        public int OrderId
        {
            get => _orderId;
            set { _orderId = value; OnPropertyChanged(); }
        }

        public bool Confirmed
        {
            get => _confirmed;
            set { _confirmed = value; OnPropertyChanged(); }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(); }
        }

        public bool IsValid => OrderId > 0;

        public static ShipmentModel FromDTO(ShipmentDTO dto)
        {
            return new ShipmentModel
            {
                ShipmentId = dto.ShipmentId,
                Confirmed = dto.Confirmed,
                CreatedAt = dto.CreatedAt,
                OrderId = dto.Order?.OrderId ?? 0
            };
        }

        public ShipmentDTO ToDTO()
        {
            return new ShipmentDTO
            {
                ShipmentId = this.ShipmentId,
                Confirmed = this.Confirmed,
                CreatedAt = this.CreatedAt,
                Order = new OrderDTO { OrderId = this.OrderId }
            };
        }
    }
}