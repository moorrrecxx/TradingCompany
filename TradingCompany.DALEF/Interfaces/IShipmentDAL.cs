using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Interfaces
{
    public interface IShipmentDAL
    {
        public ShipmentDTO Create(ShipmentDTO shipment);
        public ShipmentDTO Update(ShipmentDTO shipment);
        public void Delete(int id);
        public List<ShipmentDTO> GetAll();
        public ShipmentDTO GetById(int id);
    }
}
