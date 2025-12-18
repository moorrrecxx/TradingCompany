using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Введіть ім'я")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Введіть адресу місто/вулиця/будинок")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Введіть номер телефону +380.......")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Довжина номеру має бути рівно 13 символів")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Виберіть статус")]
        public StatusDTO? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
