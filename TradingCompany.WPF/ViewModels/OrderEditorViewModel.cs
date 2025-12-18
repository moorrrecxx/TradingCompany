using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Models;

namespace TradingCompany.WPF.ViewModels
{
    public class OrderEditorViewModel : ViewModelBase
    {
        private readonly IOrderDAL _orderDal;
        private readonly IStatusDAL _statusDal;

        private OrderModel _order;
        private string _title;

        public event Action<string> ShowMessageRequest; 
        public event Action RequestClose;

        public ObservableCollection<StatusDTO> Statuses { get; set; } = new ObservableCollection<StatusDTO>();

        public OrderModel Order
        {
            get => _order;
            set { _order = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public OrderEditorViewModel(IOrderDAL orderDal, IStatusDAL statusDal)
        {
            _orderDal = orderDal;
            _statusDal = statusDal;

            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(obj => RequestClose?.Invoke());

            LoadStatuses();
        }

        public void SetOrder(OrderDTO orderDto)
        {
            Order = OrderModel.FromDTO(orderDto);

            if (Order.OrderId == 0)
            {
                Title = "Create New Order";
                
            }
            else
            {
                Title = "Edit Order";
            }
        }

        private void LoadStatuses()
        {
            try
            {
                Statuses.Clear();
                var list = _statusDal.GetAll();
                foreach (var status in list)
                {
                    Statuses.Add(status);
                }
            }
            catch (Exception ex)
            {
                ShowMessageRequest?.Invoke($"Error loading statuses: {ex.Message}");
            }
        }

        private void ExecuteSave(object obj)
        {
            if (!Order.IsValid)
            {
                ShowMessageRequest?.Invoke("Please fill in all required fields marked in red.");
                return;
            }

            try
            {
                var dto = Order.ToDTO();

                if (dto.OrderId == 0)
                {
                    _orderDal.Create(dto);
                }
                else
                {
                    _orderDal.Update(dto);
                }

                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ShowMessageRequest?.Invoke($"Error saving order: {ex.Message}");
            }
        }
    }
}