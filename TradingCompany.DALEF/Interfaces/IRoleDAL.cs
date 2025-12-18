using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface IRoleDAL
    {
        public RoleDTO Create(RoleDTO role);
        public RoleDTO Update(RoleDTO role);
        public void Delete(int id);
        public List<RoleDTO> GetAll();
        public RoleDTO GetById(int id);
    }
}

