using DAL.Specifications;
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
        public Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity,bool>> filter = null);

        public Task<TEntity> GetById(object id);

        public Task Add(TEntity entity);

        public void Update(TEntity entityToUpdate);

        public Task<IEnumerable<TEntity>> Find(Specification<TEntity> specification);

        public Task DeleteById(object id);

        public void Delete(TEntity entity);
    }
}
