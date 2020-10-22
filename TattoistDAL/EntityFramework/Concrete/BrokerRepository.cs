using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TattoistDAL.EntityFramework.Abstract;
using TattoistDAL.Models;
using TattoistDAL.Models.Entities;

namespace TattoistDAL.EntityFramework.Concrete
{
    public class BrokerRepository : Repository<Broker>,IBrokerRepository
    {
        private readonly AppDbContext _db;
        public BrokerRepository(AppDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
    }
}
