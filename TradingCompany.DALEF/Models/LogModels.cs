using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    public class LogModels
    {
        [Key]
        [Column("log_id")] 
        public int LogId { get; set; }

        [Column("user_id")] 
        public int UserId { get; set; }

        [StringLength(255)]
        [Column("action")] 
        public string Action { get; set; } = null!;

        [Column("created_at")] 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        [ForeignKey(nameof(UserId))]
        public UserModels User { get; set; } = null!;
    }
}