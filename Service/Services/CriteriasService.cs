using AutoMapper;
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
    public class CriteriasService : ICriteriasService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CriteriasService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddCriterias(List<Criteria> criterias)
        {
            await _unitOfWork.CriteriasRepository.AddRangeAsync(criterias);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<CriteriaDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CriteriaDto>> GetByCriteriasByVacancyId(int vacancyId)
        {
            var vacancy = await _unitOfWork.TasksRepository.GetTaskByIdWithIncluded(vacancyId);
            var result = _mapper.Map<IEnumerable<CriteriaDto>>(vacancy.Criterias);
            return result;
        }
    }
}
