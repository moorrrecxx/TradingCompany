using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IAuthManager
    {
        bool Login(string username, string password);
        UserDTO CreateUser(string email, string username, string password, string roleName);
        UserDTO GetUserByLogin(string username);
        UserDTO GetUserById(int id);
        List<UserDTO> GetUsers();
        string GetRoleNameByLogin(string username);
    }
}
