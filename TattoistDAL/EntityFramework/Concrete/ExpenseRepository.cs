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
    public class ExpenseRepository : Repository<Expense>,IExpenseRepository
    {
        private readonly AppDbContext _db;
        public ExpenseRepository(AppDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
    }
}
