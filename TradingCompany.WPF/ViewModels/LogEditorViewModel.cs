using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Models;

namespace TradingCompany.WPF.ViewModels
{
    public class LogEditorViewModel : ViewModelBase
    {
        private readonly ILogDAL _logDal;
        private readonly IUserDAL _userDal;

        private LogModel _log;
        private string _title;
        public event Action<string> ShowMessageRequest;
        public event Action RequestClose;

        public ObservableCollection<UserDTO> Users { get; set; } = new ObservableCollection<UserDTO>();

        public LogModel Log
        {
            get => _log;
            set { _log = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public LogEditorViewModel(ILogDAL logDal, IUserDAL userDal)
        {
            _logDal = logDal;
            _userDal = userDal;

            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(obj => RequestClose?.Invoke());

            LoadUsers();
        }

        public void SetLog(LogDTO logDto)
        {
            Log = LogModel.FromDTO(logDto);

            if (Log.LogId == 0)
            {
                Title = "Create New Log";
            }
            else
            {
                Title = "Edit Log";
            }
        }

        private void LoadUsers()
        {
            try
            {
                Users.Clear();
                var userList = _userDal.GetAll();
                foreach (var u in userList) Users.Add(u);
            }
            catch (Exception ex)
            {
                ShowMessageRequest?.Invoke($"Error loading users: {ex.Message}");
            }
        }

        private void ExecuteSave(object obj)
        {
            if (!Log.IsValid)
            {
                ShowMessageRequest?.Invoke("Please check the fields marked in red.");
                return;
            }

            try
            {
                var dto = Log.ToDTO();

                if (dto.LogId == 0)
                    _logDal.Create(dto);
                else
                    _logDal.Update(dto , dto.LogId);

                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ShowMessageRequest?.Invoke($"Error saving log: {ex.Message}");
            }
        }
    }
}