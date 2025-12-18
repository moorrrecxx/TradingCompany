using AutoMapper;
using System;
using System.Linq;
using System.Security.Cryptography;
using TradingCompany.DALEF.Concrete;
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
            using (var context = new TradingCompanyContex(_connString))
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

        public bool ValidateUser(string login, string password)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login);
                if (user == null) return false;

                var hashedPassword = HashPassword(password, user.Salt);
                return hashedPassword.SequenceEqual(user.Password);
            }
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashAlgorithm = HashAlgorithmName.SHA512;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, 10000, hashAlgorithm);
            return pbkdf2.GetBytes(64);
        }

        public void Delete(int id)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var user = context.Users.FirstOrDefault(x=>x.UserId == id);
                if (user == null) return;
                context.Remove(user);
                context.SaveChanges();
            }
        }

        public System.Collections.Generic.List<UserDTO> GetAll()
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var allUsers = context.Users.ToList();
                return _mapper.Map<List<UserDTO>>(allUsers);
            }
        }
        public UserDTO GetById(int id)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var userEntity = context.Users.FirstOrDefault(u => u.UserId == id);
                if (userEntity == null)
                {
                    return null;
                }
                var userDto = _mapper.Map<UserDTO>(userEntity);
                return userDto;
            }
        }
        public UserDTO Update(UserDTO user)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var entity = context.Users.FirstOrDefault(x=>x.UserId == user.UserId);
                if (entity == null) return null;
                entity.Login = user.Login;
                entity.Email = user.Email;
                context.Update(entity);
                context.SaveChanges();
                return _mapper.Map<UserDTO>(entity);
            }
        }
        public UserDTO GetUserByLogin(string login)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var entity = context.Users.FirstOrDefault(u => u.Login == login);
                return _mapper.Map<UserDTO>(entity);
            }
        }

        public UserDTO GetUserByEmail(string email)
        {
            using (var context = new TradingCompanyContex(_connString))
            {
                var entity = context.Users.FirstOrDefault(u => u.Email == email);
                return _mapper.Map<UserDTO>(entity);
            }
        }
    }
}