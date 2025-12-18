using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{

    public class StatusDAL : IStatusDAL
    {
        private readonly string _connString;
        private readonly IMapper _mapper;

        public StatusDAL(string connString, IMapper mapper)
        {
            _connString = connString;
            _mapper = mapper;
        }
        public StatusDTO Create(StatusDTO status)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var newEntity = new StatusModels
                {
                    StatusName=status.StatusName

                };
                contex.Statuses.Add(newEntity);
                contex.SaveChanges();
                status.StatusId = newEntity.StatusId;
                return status;

            }
        }

        public void Delete(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = GetById(id);
                if (entity != null)
                {
                    contex.Statuses.Remove(_mapper.Map<StatusModels>(entity));
                    contex.SaveChanges();
                }
            }
        }

        public List<StatusDTO> GetAll()
        {
            using(var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Statuses.ToList();
                return _mapper.Map<List<StatusDTO>>(entity);
            }
        }

        public StatusDTO GetById(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Statuses.FirstOrDefault(a => a.StatusId == id);
                return _mapper.Map<StatusDTO>(entity);

            }
        }

        public StatusDTO Update(StatusDTO status)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Statuses.FirstOrDefault(x => x.StatusId == status.StatusId);
                if (entity != null)
                {
                    entity.StatusName = status.StatusName;

                    contex.SaveChanges();
                }
                return status;
            }
        }
    }
}
