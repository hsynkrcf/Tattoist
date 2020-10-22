using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.EntityFramework.Abstract
{
    public interface IRepository<TEntity>
         where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void RemoveToAdd(TEntity parameter);
        void Remove(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity GetFirst();
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
    }
}
