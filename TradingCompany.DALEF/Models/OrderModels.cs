using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    public class OrderModels
    {
        [Key]
        [Column("order_id")] 
        public int OrderId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("customer_name")] 
        public string CustomerName { get; set; } = null!;

        [Required]
        [StringLength(255)]
        [Column("address")] 
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(20)]
        [Column("phone")] 
        public string Phone { get; set; } = null!;

        [Column("status_id")] 
        public int? StatusId { get; set; }

        [Column("created_at")] 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        [ForeignKey(nameof(StatusId))]
        public StatusModels Status { get; set; } = null!;

        public ICollection<ShipmentModels> Shipments { get; set; } = new List<ShipmentModels>();
    }
}