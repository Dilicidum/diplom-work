using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            if(task.TaskType == TaskType.SubTask)
            {
                var baseTaskExists = await ValidateTaskExistence(task.BaseTaskId.Value);

                if(!baseTaskExists)
                {
                    return;
                }
            } 

            await _unitOfWork.TasksRepository.Add(task);
            await _unitOfWork.Save();
        }

        public async Task DeleteTask(Tasks task)
        {
            if(task.TaskType == TaskType.Task)
            {
                var subTasks = await _unitOfWork.TasksRepository.Get(x=>x.BaseTaskId == task.Id);

                if (subTasks.Any())
                {
                    foreach (var subTask in subTasks)
                    {
                        _unitOfWork.TasksRepository.Delete(subTask);
                    }
                }
            }

            _unitOfWork.TasksRepository.Delete(task);
            await _unitOfWork.Save();
        }

        public async Task UpdateTask(Tasks task)
        {
            _unitOfWork.TasksRepository.Update(task);
            await _unitOfWork.Save();
        }

        public async Task<bool> ValidateTaskExistence(int taskId)
        {
            var task = await _unitOfWork.TasksRepository.GetById(taskId);
            if(task != null)
            {
                return true;
            }    
            return false;
        }

        public async Task<IEnumerable<Tasks>> GetTasksForUser(string userId, Specification<Tasks> specification)
        {
            var tasks = (await _unitOfWork.TasksRepository.Find(specification)).Where(x=>x.UserId == userId);
            return tasks;
        }

        public async Task<Tasks> GetTaskById(string userId,int taskId)
        {
            var task = (await _unitOfWork.TasksRepository.Get(x=>x.UserId == userId && x.Id == taskId)).FirstOrDefault();

            if(task.TaskType == TaskType.Task)
            {
                task.SubTasks = (await _unitOfWork.TasksRepository.GetSubTasksForTask(taskId)).ToList();
            }

            return task;
        }
    }
}
