using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    public class RoleModels
    {
        [Key]
        [Column("role_id")] 
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("role_name")] 
        public string RoleName { get; set; } = null!;

        
        public ICollection<UserRoleModels> UserRoles { get; set; } = new List<UserRoleModels>();
    }
}