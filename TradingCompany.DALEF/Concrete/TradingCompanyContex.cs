using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Date;

namespace TradingCompany.DALEF.Concrete
{
    public class TradingCompanyContex : TradingCompanyContextOriginal
    {
        private readonly string _connString;

        public TradingCompanyContex(string connString)
        {
            _connString = connString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connString);

           
    }
}
