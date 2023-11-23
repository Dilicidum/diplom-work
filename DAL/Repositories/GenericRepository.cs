using DAL.Interfaces;
using DAL.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context) {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }


        public async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public async Task DeleteById(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity,bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            var x = query.ToList();
            
            return await query.ToListAsync();
            
        }

        public async Task<TEntity> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> Find(Specification<TEntity> spec)
        {
            return (await _context.Set<TEntity>().ToListAsync()).Where(spec.IsSatisfiedBy);

        }
    }
}
