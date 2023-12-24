using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TasksService : ITasksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskValidationService _taskValidationService;

        public TasksService(IUnitOfWork unitOfWork,ITaskValidationService taskValidationService)
        {
            _unitOfWork = unitOfWork;
            _taskValidationService = taskValidationService;
        }

        public async Task AddTask(Tasks task)
        {
            if(task.TaskType == TaskType.SubTask)
            {
                var baseTaskExists = await _taskValidationService.ValidateTaskExistence(task.BaseTaskId.Value);

                if(!baseTaskExists)
                {
                    return;
                }
            }

            await _unitOfWork.TasksRepository.AddAsync(task);
            await _unitOfWork.Save();
        }

        public async Task DeleteTask(Tasks task)
        {
            if(task.TaskType == TaskType.Task)
            {
                var spec = new SubTasksByBaseTaskIdSpec(task.Id);
                var subTasks = await _unitOfWork.TasksRepository.ListAsync(spec);

                if (subTasks.Any())
                {
                    foreach (var subTask in subTasks)
                    {
                        await _unitOfWork.TasksRepository.DeleteAsync(subTask);
                    }
                }
            }

            await _unitOfWork.TasksRepository.DeleteAsync(task);
            await _unitOfWork.Save();
        }

        public async Task UpdateTask(Tasks task)
        {
            await _unitOfWork.TasksRepository.UpdateAsync(task);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Tasks>> GetTasksForUser(TasksByTypeAndStatusAndCategorySpecAndUserId spec)
        {
            var tasks = await _unitOfWork.TasksRepository.ListAsync(spec);
            return tasks;
        }

        public async Task<Tasks> GetTaskById(string userId,int taskId)
        {

            var UserIdAndTaskIdSpec = new TaskByUserIdAndTaskIdSpec(userId,taskId);
            var task = await _unitOfWork.TasksRepository.FirstOrDefaultAsync(UserIdAndTaskIdSpec);

            return task;
        }
    }
}
