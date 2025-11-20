using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    public class StatusModels
    {
        [Key]
        [Column("status_id")] 
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("status_name")] 
        public string StatusName { get; set; } = null!;

       
        public ICollection<OrderModels> Orders { get; set; } = new List<OrderModels>();
    }
}