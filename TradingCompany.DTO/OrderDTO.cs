using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public StatusDTO? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
