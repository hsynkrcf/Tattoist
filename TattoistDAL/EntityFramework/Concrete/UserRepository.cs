using TattoistDAL.EntityFramework.Abstract;
using TattoistDAL.Models;
using TattoistDAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.EntityFramework.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public void DetachedUpdate(User user)
        {
            var detached = Get(x=>x.Id == user.Id);
            if (detached != null)
            {
                _context.Entry(detached).State = EntityState.Detached;
            }
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
