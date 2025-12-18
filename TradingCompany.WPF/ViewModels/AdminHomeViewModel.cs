using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Views; 

namespace TradingCompany.WPF.ViewModels
{
    public class AdminHomeViewModel : ViewModelBase
    {
        private readonly IOrderDAL _orderDal;
        private readonly IShipmentDAL _shipmentDal;
        private readonly IStatusDAL _statusDal;
        private readonly ILogDAL _logDal;
        private readonly IUserDAL _userDal;

        public event System.Action LogoutRequested;

        public ObservableCollection<OrderDTO> Orders { get; set; } = new ObservableCollection<OrderDTO>();
        public ObservableCollection<ShipmentDTO> Shipments { get; set; } = new ObservableCollection<ShipmentDTO>();
        public ObservableCollection<StatusDTO> Statuses { get; set; } = new ObservableCollection<StatusDTO>();
        public ObservableCollection<UserDTO> Users { get; set; } = new ObservableCollection<UserDTO>();
        public ObservableCollection<LogDTO> Logs { get; set; } = new ObservableCollection<LogDTO>();

        public ICommand LogoutCommand { get; }
        public ICommand RefreshCommand { get; }

        public ICommand AddOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }

        public ICommand AddShipmentCommand { get; }
        public ICommand EditShipmentCommand { get; }
        public ICommand DeleteShipmentCommand { get; }

        public ICommand AddStatusCommand { get; }
        public ICommand EditStatusCommand { get; }
        public ICommand DeleteStatusCommand { get; }

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }


        public ICommand AddLogCommand { get; }
        public ICommand EditLogCommand { get; }
        public ICommand DeleteLogCommand { get; } 
        public AdminHomeViewModel(IOrderDAL orderDal, IShipmentDAL shipmentDal, IStatusDAL statusDal, ILogDAL logDal, IUserDAL userDal)
        {
            _orderDal = orderDal;
            _shipmentDal = shipmentDal;
            _statusDal = statusDal;
            _logDal = logDal;
            _userDal = userDal;

            LogoutCommand = new RelayCommand(obj => LogoutRequested?.Invoke());
            RefreshCommand = new RelayCommand(obj => LoadData());

            AddOrderCommand = new RelayCommand(obj => OpenOrderEditor(new OrderDTO()));
            EditOrderCommand = new RelayCommand(obj => { if (obj is OrderDTO val) OpenOrderEditor(val); });
            DeleteOrderCommand = new RelayCommand(obj => { if (obj is OrderDTO val) { _orderDal.Delete(val.OrderId); LoadData(); } });

            AddShipmentCommand = new RelayCommand(obj => OpenShipmentEditor(new ShipmentDTO()));
            EditShipmentCommand = new RelayCommand(obj => { if (obj is ShipmentDTO val) OpenShipmentEditor(val); });
            DeleteShipmentCommand = new RelayCommand(obj => { if (obj is ShipmentDTO val) { _shipmentDal.Delete(val.ShipmentId); LoadData(); } });

            AddStatusCommand = new RelayCommand(obj => OpenStatusEditor(new StatusDTO()));
            EditStatusCommand = new RelayCommand(obj => { if (obj is StatusDTO val) OpenStatusEditor(val); });
            DeleteStatusCommand = new RelayCommand(obj => { if (obj is StatusDTO val) { _statusDal.Delete(val.StatusId); LoadData(); } });

            AddUserCommand = new RelayCommand(obj => OpenUserEditor(new UserDTO()));
            EditUserCommand = new RelayCommand(obj => { if (obj is UserDTO val) OpenUserEditor(val); });
            DeleteUserCommand = new RelayCommand(obj => { if (obj is UserDTO val) { _userDal.Delete(val.UserId); LoadData(); } });

            DeleteLogCommand = new RelayCommand(obj => { if (obj is LogDTO val) { _logDal.Delete(val.LogId); LoadData(); } });

            AddLogCommand = new RelayCommand(obj => OpenLogEditor(new LogDTO()));
            EditLogCommand = new RelayCommand(obj => { if (obj is LogDTO val) OpenLogEditor(val); });

            LoadData();
        }

        private void OpenLogEditor(LogDTO log)
        {
            var vm = new LogEditorViewModel(_logDal, _userDal);
            vm.SetLog(log);
            var win = new LogEditorView { DataContext = vm };
            vm.RequestClose += () => { win.Close(); LoadData(); };
            win.ShowDialog();
        }
        private void OpenOrderEditor(OrderDTO order)
        {
            var vm = new OrderEditorViewModel(_orderDal, _statusDal);
            vm.SetOrder(order);
            var win = new OrderEditorView { DataContext = vm };
            vm.RequestClose += () => { win.Close(); LoadData(); };
            win.ShowDialog();
        }

        private void OpenShipmentEditor(ShipmentDTO shipment)
        {
            var vm = new ShipmentEditorViewModel(_shipmentDal, _orderDal);
            vm.SetShipment(shipment);
            var win = new ShipmentEditorView { DataContext = vm };
            vm.RequestClose += () => { win.Close(); LoadData(); };
            win.ShowDialog();
        }

        private void OpenStatusEditor(StatusDTO status)
        {
            var vm = new StatusEditorViewModel(_statusDal);
            vm.SetStatus(status);
            var win = new StatusEditorView { DataContext = vm };
            vm.CloseRequest += () => { win.Close(); LoadData(); };
            win.ShowDialog();
        }

        private void OpenUserEditor(UserDTO user)
        {
            var vm = new UserEditorViewModel(_userDal);
            vm.SetUser(user);
            var win = new UserEditorView { DataContext = vm };
            vm.CloseRequest += () => { win.Close(); LoadData(); };
            win.ShowDialog();
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