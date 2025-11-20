using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class ShipmentDTO
    {
        public int ShipmentId { get; set; }
        public OrderDTO? Order { get; set; }
        public DateTime  CreatedAt { get; set; } = DateTime.Now;
        public bool Confirmed { get; set; }

    }
}
