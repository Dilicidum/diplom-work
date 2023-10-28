using DAL.Interfaces;
using DAL.Models;
using DAL.Specifications;
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

        public Task<bool> ValidateTaskExistence(int? taskId, TaskType? taskType);

        public Task<IEnumerable<Tasks>> GetTasksForUser(string userId, Specification<Tasks> specification);
    }
}
