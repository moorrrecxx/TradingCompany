using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class LogDAL : ILogDAL
    {
        private readonly string _connString;
        private readonly IMapper _mapper;

        public LogDAL(string connString, IMapper mapper)
        {
            _connString = connString;
            _mapper = mapper;
        }

        public LogDTO Create(LogDTO log)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var newEntity = new LogModels
                {
                    UserId = log.User.UserId,
                    Action = log.Action
                };
                contex.Logs.Add(newEntity);
                contex.SaveChanges();
                log.LogId = newEntity.LogId;
                return log;

            }
        }

        public void Delete(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = GetById(id);
                if (entity != null)
                {
                    contex.Logs.Remove(_mapper.Map<LogModels>(entity));
                    contex.SaveChanges();
                }
            }
        }

        public List<LogDTO> GetAll()
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Logs.Include(x => x.User).ToList();
                return _mapper.Map<List<LogDTO>>(entity);
            }
        }

        public LogDTO GetById(int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Logs.FirstOrDefault(a => a.LogId == id);
                return _mapper.Map<LogDTO>(entity);

            }
        }

        public LogDTO Update(LogDTO log,int id)
        {
            using (var contex = new TradingCompanyContex(_connString))
            {
                var entity = contex.Logs.FirstOrDefault(l => l.LogId == id);

                if (entity != null)
                {
                    entity.Action = log.Action;

                    if (log.User != null)
                    {
                        entity.UserId = log.User.UserId;
                    }

                    contex.SaveChanges();
                }

                return log;
            }
        }
    }
}
