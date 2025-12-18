using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IOrderManager
    {
        public List<OrderDTO> GetAllOrders();
        public OrderDTO CreateOrder(OrderDTO product);
        public void DeleteOrder(int productId);
        public OrderDTO UpdateOrder(OrderDTO product);
        public List<StatusDTO> GetAllStatuses();
        object GetOrderById(int id);
    }
}
