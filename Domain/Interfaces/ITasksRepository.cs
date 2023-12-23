using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITasksRepository:IGenericRepository<Tasks>
    {
        public Task<IEnumerable<Tasks>> GetDueTasksForToday(string userId);

        public Task<IEnumerable<Tasks>> GetSubTasksForTask(int taskId);

    }
}
