using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concreate
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserDAL _userDAL;
        private readonly IUserRoleDAL _userRoleDAL;
        public AuthManager (IUserDAL userDAL, IUserRoleDAL userRoleDAL)
        {
            _userDAL= userDAL;
            _userRoleDAL = userRoleDAL;

        }
        public UserDTO CreateUser(string email, string username, string password, string roleName)
        {
            var tim = new UserDTO
            {

                Email = email,
                Login = username
            };
            var user = _userDAL.Create(tim, password);
            if (user == null || user.UserId <= 0)
            {
                throw new Exception("User creation failed.");
            }
            _userRoleDAL.AddUserToRole(user.UserId, 1);

            return _userDAL.GetUserByEmail(user.Email);
        }

        public string GetRoleNameByLogin(string username)
        {
            return _userRoleDAL.GetRoleNameByLogin(username);
        }

        public UserDTO GetUserById(int id)
        {
            return _userDAL.GetById(id);
        }

        public UserDTO GetUserByLogin(string username)
        {
            return _userDAL.GetUserByLogin(username);
        }

        public List<UserDTO> GetUsers()
        {
            return _userDAL.GetAll();
        }

        public bool Login(string username, string password)
        {
           return _userDAL.ValidateUser(username, password);
        }
    }
}
