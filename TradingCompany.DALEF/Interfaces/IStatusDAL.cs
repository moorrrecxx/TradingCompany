using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface IStatusDAL
    {
        public StatusDTO Create(StatusDTO status);
        public StatusDTO Update(StatusDTO status);
        public void Delete(int id);
        public List<StatusDTO> GetAll();
        public StatusDTO GetById(int id);
    }
}
