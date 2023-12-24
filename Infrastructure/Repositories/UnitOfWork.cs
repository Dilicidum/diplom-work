using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly ITasksRepository _taskRepository;
        public UnitOfWork(ApplicationContext context, ITasksRepository taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }

        public ITasksRepository TasksRepository { 
                get
                {
                    return _taskRepository;
                } 
            }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
