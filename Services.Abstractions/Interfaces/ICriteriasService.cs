using Domain.Entities;
using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICriteriasService
    {
        Task<IEnumerable<CriteriaDto>> GetAll();

        Task<IEnumerable<CriteriaDto>> GetByCriteriasByVacancyId(int vacancyId);

        Task AddCriterias(List<Criteria> criterias);
    }
}
