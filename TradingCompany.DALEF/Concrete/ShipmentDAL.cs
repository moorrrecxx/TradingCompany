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
    public class ShipmentDAL : IShipmentDAL
    {
        private readonly string _connString;
        private readonly IMapper _mapper;

        public ShipmentDAL(string connString, IMapper mapper)
        {
            _connString = connString;
            _mapper = mapper;
        }

        public ShipmentDTO Create(ShipmentDTO shipment)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var newEntity = new ShipmentModels
                {
                    OrderId = shipment.Order.OrderId,
                    Confirmed = shipment.Confirmed
                };
                contex.Shipments.Add(newEntity);
                contex.SaveChanges();
                shipment.ShipmentId = newEntity.ShipmentId;
                return shipment;
            }
        }

        public void Delete(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Shipments.FirstOrDefault(s => s.ShipmentId == id);
                if (entity != null)
                {
                    contex.Shipments.Remove(entity);
                    contex.SaveChanges();
                }
            }
        }

        public List<ShipmentDTO> GetAll()
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Shipments.Include(x => x.Order).ToList();
                return _mapper.Map<List<ShipmentDTO>>(entity);
            }
        }

        public ShipmentDTO GetById(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Shipments.FirstOrDefault(a => a.ShipmentId == id);
                return _mapper.Map<ShipmentDTO>(entity);
            }
        }

        public ShipmentDTO Update(ShipmentDTO shipment)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Shipments.FirstOrDefault(s => s.ShipmentId == shipment.ShipmentId);

                if (entity != null)
                {
                    if (shipment.Order != null && shipment.Order.OrderId > 0)
                    {
                        entity.OrderId = shipment.Order.OrderId;
                    }

                    entity.Confirmed = shipment.Confirmed;

                    contex.SaveChanges();
                }

                return shipment;
            }
        }
    }
}