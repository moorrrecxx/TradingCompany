using TradingCompany.DALEF.Concrete;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.WPF.Services.Interfaces;

namespace TradingCompany.WPF.Services.Concrete
{
    public class Authentication : IAuthentication
    {
        private readonly IUserDAL _userDal;
        private readonly IUserRoleDAL _userRoleDal;
        public Authentication(IUserDAL userDal, IUserRoleDAL userRoleDal)
        {
            _userDal = userDal;
            _userRoleDal = userRoleDal;
        }

        public bool ValidateUser(string login, string password, out string role)
        {
            role = "User"; 

            bool isValid = _userDal.ValidateUser(login, password);

            if (isValid)
            {
                role = _userRoleDal.GetRoleNameByLogin(login);
                return true;
            }

            return false;
        }
    }
}