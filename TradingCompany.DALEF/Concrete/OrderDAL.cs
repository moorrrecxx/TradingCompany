using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class OrderDAL : IOrderDAL
    {
        private readonly string _connString;
        private readonly IMapper _mapper;

        public OrderDAL(string connString, IMapper mapper)
        {
            _connString = connString;
            _mapper = mapper;
        }

        public OrderDTO Create(OrderDTO order)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var newEntity = new OrderModels
                {
                    CustomerName = order.CustomerName,
                    Address = order.Address,
                    Phone = order.Phone,
                    StatusId = order.Status.StatusId
                };
                contex.Orders.Add(newEntity);
                contex.SaveChanges();
                order.OrderId = newEntity.OrderId;
                return order;
            }
        }

        public void Delete(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Orders.FirstOrDefault(x => x.OrderId == id);
                if (entity != null)
                {
                    contex.Orders.Remove(entity);
                    contex.SaveChanges();
                }
            }
        }

        public List<OrderDTO> GetAll()
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Orders.Include(x => x.Status).ToList();
                return _mapper.Map<List<OrderDTO>>(entity);
            }
        }

        public OrderDTO GetById(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Orders.Include(x => x.Status).FirstOrDefault(a => a.OrderId == id);
                return _mapper.Map<OrderDTO>(entity);
            }
        }

        public OrderDTO Update(OrderDTO order)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);

                if (entity != null)
                {
                    if (order.Status != null && order.Status.StatusId > 0)
                    {
                        entity.StatusId = order.Status.StatusId;
                    }

                    if (!string.IsNullOrWhiteSpace(order.CustomerName))
                        entity.CustomerName = order.CustomerName;

                    if (!string.IsNullOrWhiteSpace(order.Phone))
                        entity.Phone = order.Phone;

                    if (!string.IsNullOrWhiteSpace(order.Address))
                        entity.Address = order.Address;

                    contex.SaveChanges();
                }

                return order;
            }
        }
    }
}