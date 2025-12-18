using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Models;

namespace TradingCompany.WPF.ViewModels
{
    public class ShipmentEditorViewModel : ViewModelBase
    {
        private readonly IShipmentDAL _shipmentDal;
        private readonly IOrderDAL _orderDal;

        private ShipmentModel _shipment;
        private string _title;
        public event Action<string, string> ShowMessageRequest; 
        public event Action<string, string> ShowErrorRequest;   
        public event Action RequestClose;                      

        public ObservableCollection<OrderDTO> Orders { get; set; } = new ObservableCollection<OrderDTO>();

        public ShipmentModel Shipment
        {
            get => _shipment;
            set { _shipment = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ShipmentEditorViewModel(IShipmentDAL shipmentDal, IOrderDAL orderDal)
        {
            _shipmentDal = shipmentDal;
            _orderDal = orderDal;

            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

            LoadOrders();
        }

        public void SetShipment(ShipmentDTO shipmentDto)
        {
            Shipment = ShipmentModel.FromDTO(shipmentDto);

            if (Shipment.ShipmentId == 0)
            {
                Title = "Create New Shipment";

            }
            else
            {
                Title = "Edit Shipment";
            }
        }

        private void LoadOrders()
        {
            Orders.Clear();
            foreach (var o in _orderDal.GetAll())
            {
                Orders.Add(o);
            }
        }

        private void ExecuteSave(object obj)
        {
            if (Shipment == null) return;

            if (!Shipment.IsValid)
            {
 
                ShowMessageRequest?.Invoke("Please select an Order.", "Validation Error");
                return;
            }

            try
            {
                var dto = Shipment.ToDTO();

                if (dto.ShipmentId == 0)
                    _shipmentDal.Create(dto);
                else
                    _shipmentDal.Update(dto);


                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ShowErrorRequest?.Invoke($"Error saving shipment: {ex.Message}", "Database Error");
            }
        }
    }
}