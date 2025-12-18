using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface IUserRoleDAL
    {
        public UserRoleDTO Create(UserRoleDTO userRole);
        public UserRoleDTO Update(UserRoleDTO userRole);
        public void Delete(int id);
        public List<UserRoleDTO> GetAll();
        public UserRoleDTO GetById(int id);

        string GetRoleNameByLogin(string login);

        void AddUserToRole(int userId, int roleId);
    }
}
