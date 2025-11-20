using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface IUserDAL
    {
        public UserDTO Create(UserDTO user, string password);
        public UserDTO Update(UserDTO user);
        public void  Delete(int id);
        public List<UserDTO> GetAll ();
        public UserDTO GetById (int id);
    }
}
