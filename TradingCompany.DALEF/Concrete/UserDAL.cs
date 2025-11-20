using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class UserDAL : IUserDAL
    {
        private readonly string _connString;
        private readonly IMapper _mapper;

        public UserDAL(string connString, IMapper mapper)
        {
            _connString = connString;
            _mapper = mapper;
        }

        public UserDTO Create(UserDTO user, string password)
        {

            using(var context = new TradingCompanyContex(_connString))
            {
                Guid saltGuid = Guid.NewGuid();
                var saltBytes = saltGuid.ToByteArray();
                var entity = new UserModels
                {
                    Login = user.Login,
                    Email = user.Email,
                    Password = HashPassword(password, saltBytes),
                    Salt = saltBytes,

                };
                context.Users.Add(entity);
                context.SaveChanges();
                user.UserId = entity.UserId;
                return user;
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserDTO Update(UserDTO user)
        {
            throw new NotImplementedException();
        }
        public byte[] HashPassword(string password, byte[] salt)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            var hashAlgorithm = HashAlgorithmName.SHA512;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, 10000, hashAlgorithm);
            return pbkdf2.GetBytes(64);
        }
            

    }
}
