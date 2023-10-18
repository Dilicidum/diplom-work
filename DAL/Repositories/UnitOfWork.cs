using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly IGenericRepository<Tasks> _taskRepository;
        public UnitOfWork(ApplicationContext context, IGenericRepository<Tasks> taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }

        public IGenericRepository<Tasks> TasksRepository { 
                get
                {
                    return _taskRepository;
                } 
            }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
