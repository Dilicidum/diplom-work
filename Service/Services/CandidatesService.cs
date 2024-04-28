using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CandidatesService : ICandidatesService
    {
        private IUnitOfWork _unitOfWork;

        public CandidatesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task CreateCandidate(int vacancyId, Candidate candidate)
        {
            candidate.VacancyId = vacancyId;
            var temp = candidate.CandidateCriterias;
            candidate.CandidateCriterias = null;
            var res = (await _unitOfWork.CandidatesRepository.AddAsync(candidate));
            res.CandidateCriterias = temp;
            await _unitOfWork.Save();
        }

        public async Task DeleteCandidate(int candidateId)
        {
            var model = await _unitOfWork.CandidatesRepository.GetById(candidateId);
            model.CandidateCriterias = null;
            await _unitOfWork.CandidatesRepository.DeleteAsync(model);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Candidate>> GetCandidatesForVacancy(int vacancyId)
        {
            var res = (await _unitOfWork.TasksRepository.GetTaskByIdWithIncluded(vacancyId)).Candidates;
            return res;
        }

        public async Task<double[,]> GetCriteriasForCandidatesForVacancy(int vacancyId)
        {
            var candidates = (await _unitOfWork.CandidatesRepository.GetCandidatesByVacancyId(vacancyId)).ToList();
            
            var res = new double[candidates.Count,9];
            for(int i = 0; i < candidates.Count; i++)
            {
                candidates[i].CandidateCriterias.OrderBy(x=>x.CriteriaId);
                for(int j = 0; j < 9; j++)
                {
                    res[i,j] = candidates[i].CandidateCriterias[j].Value;
                }
            }
            return res;
        }
    }
}
