using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concreate
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderDAL orderDAL;
        private readonly IStatusDAL statusDAL;

        public OrderManager(IOrderDAL orderDAL, IStatusDAL statusDAL)
        {
            this.orderDAL = orderDAL;
            this.statusDAL = statusDAL;
        }

        public OrderDTO CreateOrder(OrderDTO product)
        {
            return orderDAL.Create(product);
        }

        public void DeleteOrder(int productId)
        {
            orderDAL.Delete(productId);
        }

        public List<OrderDTO> GetAllOrders()
        {
            return orderDAL.GetAll();
        }

        public List<StatusDTO> GetAllStatuses()
        {
            return statusDAL.GetAll();
        }

        public object GetOrderById(int id)
        {
            return orderDAL.GetById(id);
        }

        public OrderDTO UpdateOrder(OrderDTO product)
        {
            return orderDAL.Update(product);
        }
    }
}
