using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface IOrderDAL
    {
        public OrderDTO Create(OrderDTO order);
        public OrderDTO Update(OrderDTO order);
        public void Delete(int id);
        public List<OrderDTO> GetAll();
        public OrderDTO GetById(int id);
    }
}
