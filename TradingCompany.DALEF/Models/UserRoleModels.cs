using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DALEF.Models
{
    [Table("User_Roles")]
    public class UserRoleModels
    {
        [Key]
        [Column("user_roles_id")] 
        public int UserRolesId { get; set; }

        [Column("user_id")] 
        public int UserId { get; set; }

        [Column("role_id")] 
        public int RoleId { get; set; }

        
        [ForeignKey(nameof(UserId))]
        public UserModels User { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public RoleModels Role { get; set; } = null!;
    }
}