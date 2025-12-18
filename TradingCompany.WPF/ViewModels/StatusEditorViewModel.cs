using System;
using System.Windows;
using System.Windows.Input;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Core;
using TradingCompany.WPF.Models; 

namespace TradingCompany.WPF.ViewModels
{
    public class StatusEditorViewModel : ViewModelBase
    {
        private readonly IStatusDAL _statusDal;

        public StatusModel Status { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseRequest;

        public StatusEditorViewModel(IStatusDAL statusDal)
        {
            _statusDal = statusDal;
            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(obj => CloseRequest?.Invoke());
        }

        public void SetStatus(StatusDTO statusDto)
        {
            Status = new StatusModel
            {
                StatusId = statusDto.StatusId,
                Name = statusDto.StatusName 
            };

            if (Status.StatusId == 0)
            {
                Title = "Create New Status";
            }
            else
            {
                Title = "Edit Status";
            }

            OnPropertyChanged(nameof(Status));
        }

        private void ExecuteSave(object obj)
        {

            if (!Status.IsValid)
            {
                MessageBox.Show("Please fix the errors before saving.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var dto = Status.ToDTO();

                if (dto.StatusId == 0)
                    _statusDal.Create(dto);
                else
                    _statusDal.Update(dto);

                CloseRequest?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}