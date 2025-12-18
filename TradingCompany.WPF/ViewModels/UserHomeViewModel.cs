using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;

namespace TradingCompany.WPF.ViewModels
{
    public class UserHomeViewModel : ViewModelBase
    {
        private readonly IOrderDAL _orderDal;
        private readonly IShipmentDAL _shipmentDal;
        private readonly IStatusDAL _statusDal;
        private readonly IUserDAL _userDal;
        private readonly ILogDAL _logDal;

        public event System.Action LogoutRequested;

        public ObservableCollection<OrderDTO> Orders { get; set; } = new ObservableCollection<OrderDTO>();
        public ObservableCollection<ShipmentDTO> Shipments { get; set; } = new ObservableCollection<ShipmentDTO>();
        public ObservableCollection<StatusDTO> Statuses { get; set; } = new ObservableCollection<StatusDTO>();
        public ObservableCollection<UserDTO> Users { get; set; } = new ObservableCollection<UserDTO>();
        public ObservableCollection<LogDTO> Logs { get; set; } = new ObservableCollection<LogDTO>();

        public ICommand LogoutCommand { get; }
        public ICommand RefreshCommand { get; }

        public UserHomeViewModel(IOrderDAL orderDal, IShipmentDAL shipmentDal, IStatusDAL statusDal, IUserDAL userDal, ILogDAL logDal)
        {
            _orderDal = orderDal;
            _shipmentDal = shipmentDal;
            _statusDal = statusDal;
            _userDal = userDal;
            _logDal = logDal;

            LogoutCommand = new RelayCommand(obj => LogoutRequested?.Invoke());
            RefreshCommand = new RelayCommand(obj => LoadData());

            LoadData();
        }

        private void LoadData()
        {
            Orders.Clear();
            foreach (var x in _orderDal.GetAll()) Orders.Add(x);

            Shipments.Clear();
            foreach (var x in _shipmentDal.GetAll()) Shipments.Add(x);

            Statuses.Clear();
            foreach (var x in _statusDal.GetAll()) Statuses.Add(x);

            Users.Clear();
            foreach (var x in _userDal.GetAll()) Users.Add(x);

            Logs.Clear();
            foreach (var x in _logDal.GetAll()) Logs.Add(x);
        }
    }
}