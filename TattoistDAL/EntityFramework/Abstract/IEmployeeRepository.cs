using TattoistDAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TattoistDAL.EntityFramework.Abstract;

namespace TattoistDAL.EntityFramework.Abstract
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
    }
}
