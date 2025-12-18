using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface ILogDAL
    {
        public LogDTO Create(LogDTO log);
        public LogDTO Update(LogDTO log,int id);
        public void Delete(int id);
        public List<LogDTO> GetAll();
        public LogDTO GetById(int id);
    }
}
