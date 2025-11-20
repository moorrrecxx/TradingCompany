using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    [Index(nameof(Login), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserModels
    {
        [Key]
        [Column("user_id")] 
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("login")] 
        public string Login { get; set; } = null!;

        [Required]
        [Column("password", TypeName = "binary(64)")] 
        public byte[] Password { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [Column("email")] 
        public string Email { get; set; } = null!;

        [Required]
        [Column("salt", TypeName = "binary(16)")] 
        public byte[] Salt { get; set; } = null!;

        [Column("created_at")] 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")] 
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        
        public ICollection<LogModels> Logs { get; set; } = new List<LogModels>();
        public ICollection<UserRoleModels> UserRoles { get; set; } = new List<UserRoleModels>();
    }
}