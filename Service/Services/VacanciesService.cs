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
    public class VacanciesService : IVacanciesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskValidationService _taskValidationService;

        public VacanciesService(IUnitOfWork unitOfWork,ITaskValidationService taskValidationService)
        {
            _unitOfWork = unitOfWork;
            _taskValidationService = taskValidationService;
        }

        public async Task AddVacancy(Vacancy task)
        {
            await _unitOfWork.TasksRepository.AddAsync(task);
            await _unitOfWork.Save();
        }


        
        public async Task DeleteVacancy(Vacancy task)
        {

            await _unitOfWork.TasksRepository.DeleteAsync(task);
            await _unitOfWork.Save();
        }

        public async Task UpdateVacancy(Vacancy task)
        {
            var res = await _unitOfWork.TasksRepository.GetTaskByIdWithIncluded(task.Id);
            res.Description = task.Description;
            res.Name = task.Name;
            res.Status = task.Status;
            res.DueDate = task.DueDate;
            var criterias = await _unitOfWork.CriteriasRepository.ListAsync();
            var criteariasToChange = criterias.Where(x=>x.VacancyId == task.Id).ToList();
            for(int i = 0; i < criteariasToChange.Count(); i++)
            {
                criteariasToChange[i].Name = task.Criterias[i].Name;
                criteariasToChange[i].VacancyWeight = task.Criterias[i].VacancyWeight;
            }
            res.Criterias = criteariasToChange;
            //res.Criterias = task.Criterias;
            //if(task.Criterias.Count > 0)
            //{
            //    var criterias = await _unitOfWork.CriteriasRepository.ListAsync();
            //    var criteariasToChange = criterias.Where(x=>x.VacancyId == task.Id);
            //    await _unitOfWork.CriteriasRepository.DeleteRangeAsync(criteariasToChange);
            //}

            await _unitOfWork.TasksRepository.UpdateAsync(res);
        }

        public async Task<IEnumerable<Vacancy>> GetVacanciesForUser(VacancyByTypeAndStatusAndCategorySpecAndUserId spec)
        {
            var tasks = await _unitOfWork.TasksRepository.ListAsync(spec);
            return tasks;
        }

        public async Task<Vacancy> GetVacancyById(string userId,int taskId)
        {

            var UserIdAndTaskIdSpec = new VacancyByUserIdAndTaskIdSpec(userId,taskId);
            var task = await _unitOfWork.TasksRepository.FirstOrDefaultAsync(UserIdAndTaskIdSpec);

            return task;
        }
    }
}
