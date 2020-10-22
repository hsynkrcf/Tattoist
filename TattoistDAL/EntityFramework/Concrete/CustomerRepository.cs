using TattoistDAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TattoistDAL.EntityFramework.Concrete;
using TattoistDAL.Models;
using TattoistDAL.EntityFramework.Abstract;

namespace TattoistDAL.EntityFramework.Concrete
{
    public class CustomerRepository : Repository<Customer>,ICustomerRepository
    {
        private readonly AppDbContext _db;
        public CustomerRepository(AppDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

    }
}
