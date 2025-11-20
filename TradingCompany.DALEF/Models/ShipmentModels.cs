using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    public class ShipmentModels
    {
        [Key]
        [Column("shipment_id")] 
        public int ShipmentId { get; set; }

        [Column("order_id")] 
        public int OrderId { get; set; }

        [Column("created_at")] 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        [Required]
        [Column("confirmed")] 
        public bool Confirmed { get; set; }

        
        [ForeignKey(nameof(OrderId))]
        public OrderModels Order { get; set; } = null!;
    }
}