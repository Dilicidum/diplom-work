using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AnalysisService : IAnalysisService
    {
        private IUnitOfWork _unitOfWork;

        public AnalysisService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task RecordAnalysis(int vacancyId, List<int> candidatesId)
        {
            int i = 0;
            foreach(var candidate in candidatesId)
            {
                var vacancy = await _unitOfWork.TasksRepository.GetTaskByIdWithIncluded(vacancyId);
                var candidates = vacancy.Candidates;

                var Analysis = new Analysis() { CandidateId = candidates[candidatesId[i]].Id-1, VacancyId = vacancyId };
                await _unitOfWork.AnalysisRepository.AddAsync(Analysis);
                i++;
            }
        }

        public async Task<IEnumerable<string>> GetAnalysisForVacancy(int vacancyId)
        {
            var vacancy = await _unitOfWork.TasksRepository.GetTaskByIdWithIncluded(vacancyId);
            var result = new List<string>();
            var candidateIds = vacancy.Analyses.Select(x=>x.CandidateId);
            foreach( var candidate in candidateIds)
            {
                result.Add(vacancy.Candidates.Where(x=>x.Id == candidate).FirstOrDefault().Email);
            }
            return result;
        }
    }
}
