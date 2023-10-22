using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITasksService
    {
        public Task AddTask(Tasks task);

        public Task<IEnumerable<Tasks>> GetTasksForUser(string userId,Func<Tasks, bool>? filter = null);

        public Task DeleteTask(Tasks task);

        public Task UpdateTask(Tasks task);
    }
}
