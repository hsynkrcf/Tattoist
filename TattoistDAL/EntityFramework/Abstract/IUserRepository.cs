using TattoistDAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.EntityFramework.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        void DetachedUpdate(User user);
    }
}
