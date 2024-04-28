using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Interfaces
{
    public interface ICandidatesService
    {
        public Task CreateCandidate(int vacancyId, Candidate candidate);

        public Task<IEnumerable<Candidate>> GetCandidatesForVacancy(int vacancyId);

        public Task DeleteCandidate(int candidateId);

        public Task<double[,]> GetCriteriasForCandidatesForVacancy(int vacancyId);
    }
}
