using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity,bool>> filter = null,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        public Task<TEntity> GetById(object id);

        public Task Add(TEntity entity);

        public void Update(TEntity entityToUpdate);

        public void DeleteById(object id);

        public void Delete(TEntity entity);
    }
}
