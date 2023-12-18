using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TasksRepository : GenericRepository<Tasks>, ITasksRepository
    {
        public TasksRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tasks>> GetDueTasksForToday(string userId)
        {
            var tasks = await _context.Tasks.Where(x=>x.DueDate == DateTime.Today && x.UserId == userId).ToListAsync();
            return tasks;
        }

        public async Task<IEnumerable<Tasks>> GetSubTasksForTask(int taskId)
        {
            var tasks = (await _context.Tasks.Where(x=>x.Id == taskId).Include(x=>x.SubTasks).FirstOrDefaultAsync()).SubTasks;
            return tasks;
        }
    }
}
