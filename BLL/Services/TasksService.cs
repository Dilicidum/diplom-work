using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TasksService : ITasksService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TasksService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddTask(Tasks task)
        {
            await _unitOfWork.TasksRepository.Add(task);
            await _unitOfWork.Save();
        }

        public async Task DeleteTask(Tasks task)
        {
            var subTasks = await _unitOfWork.TasksRepository.Get(x=>x.BaseTaskId == task.BaseTaskId);

            if (subTasks.Any())
            {
                foreach (var subTask in subTasks)
                {
                    _unitOfWork.TasksRepository.Delete(subTask);
                }
            }

            _unitOfWork.TasksRepository.Delete(task);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Tasks>> GetTasksForUser(string userId)
        {
            var tasks = await _unitOfWork.TasksRepository.Get(x=>x.UserId == userId);

            return tasks;
        }
    }
}
