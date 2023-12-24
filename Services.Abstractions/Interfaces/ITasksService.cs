using Domain.Entities;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Interfaces
{
    public interface ITasksService
    {
        public Task AddTask(Tasks task);

        public Task DeleteTask(Tasks task);

        public Task UpdateTask(Tasks task);

        public Task<IEnumerable<Tasks>> GetTasksForUser(TasksByTypeAndStatusAndCategorySpecAndUserId spec);

        public Task<Tasks> GetTaskById(string userId,int taskId);
    }
}
