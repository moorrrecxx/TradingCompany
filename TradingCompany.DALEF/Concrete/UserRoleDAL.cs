using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class UserRoleDAL : IUserRoleDAL
    {
        private readonly string _connString;
        private readonly IMapper _mapper;

        public UserRoleDAL(string connString, IMapper mapper)
        {
            _connString = connString;
            _mapper = mapper;
        }
        public UserRoleDTO Create(UserRoleDTO userRole)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public string GetRoleNameByLogin(string login)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login);
                if (user == null) return "User"; 
                var userRole = context.UserRoles
                                      .FirstOrDefault(ur => ur.UserId == user.UserId);
                var role = "";
                if(userRole.RoleId==2)
                {
                    return role = "Admin";
                }
                return role = "User";
                
            }
        }

        public void AddUserToRole(int userId, int roleId)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var link = new UserRoleModels 
                {
                    UserId = userId,
                    RoleId = roleId
                };
                context.UserRoles.Add(link);
                context.SaveChanges();
            }
        }

        public UserRoleDTO Update(UserRoleDTO userRole)
        {
            throw new NotImplementedException();
        }

        public List<UserRoleDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserRoleDTO GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
