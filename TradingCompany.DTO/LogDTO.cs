using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class LogDTO
    {
        public int LogId { get; set; }
        public  UserDTO? User { get; set; }
        public string? Action {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
