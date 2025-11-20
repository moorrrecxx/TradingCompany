using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class UserRoleDTO
    {
        public int UserRoleId;
        public List<UserDTO>? User { get; set; }
        public List<RoleDTO>? Role { get; set; }
    }
}
