using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICandidatesRepository : IReadRepositoryBase<Candidate>, IRepositoryBase<Candidate>
    {
        public Task<Candidate>  GetById(int id);

        public Task <IEnumerable<Candidate>> GetCandidatesByVacancyId(int vacancyId);
    }
}
