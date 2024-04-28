using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using API.Interfaces;
using Services.Abstractions.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Services.Abstractions.Interfaces;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CriteriasController : ControllerBase
    {
        private readonly ICriteriasService _criteriasService;
        private readonly ICandidatesService _candidatesService;
        private IMapper _mapper;

        public CriteriasController(ICriteriasService criteriasService,ICandidatesService candidatesService, IMapper mapper = null)
        {
            _criteriasService = criteriasService;
            _candidatesService = candidatesService;
            _mapper = mapper;
        }

        [HttpGet("{vacancyId}")]
        public async Task<IActionResult> GetCriteriasForVacancy(int vacancyId)
        {
            var res = await _criteriasService.GetByCriteriasByVacancyId(vacancyId);
            return Ok(res);
        }

        [HttpPost("{vacancyId}")]
        public async Task<IActionResult> CreateCandidate(int vacancyId,CandidateDTO candidate)
        {
            var model = _mapper.Map<Candidate>(candidate);
            var candidateCriterias = _mapper.Map<IEnumerable<CandidateCriteria>>(candidate.CandidateCriterias);
            model.CandidateCriterias = candidateCriterias.ToList();
            await _candidatesService.CreateCandidate(vacancyId, model);
            var res = await _criteriasService.GetByCriteriasByVacancyId(vacancyId);
            return Ok(res);
        }
    }
}
