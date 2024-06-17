using Domain.Entities;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Interfaces
{
    public interface IVacanciesService
    {
        public Task AddVacancy(Vacancy task);

        public Task DeleteVacancy(Vacancy task);

        public Task UpdateVacancy(Vacancy task);

        public Task<IEnumerable<Vacancy>> GetVacanciesForUser(VacancyByTypeAndStatusAndCategorySpecAndUserId spec);

        public Task<Vacancy> GetVacancyById(string userId,int taskId);
    }
}
