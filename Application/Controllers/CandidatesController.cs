using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions.Interfaces;
using System.Net.WebSockets;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private ICandidatesService _candidatesService;
        private IMapper _mapper;

        public CandidatesController(ICandidatesService candidatesService,IMapper mapper)
        {
            _candidatesService = candidatesService;
            _mapper = mapper;
        }

        [HttpGet("{vacancyId}")]
        public async Task<IActionResult> GetCandidatesForVacancy(int vacancyId)
        {
            var res = await _candidatesService.GetCandidatesForVacancy(vacancyId);
            var res1 = _mapper.Map<IEnumerable<CandidateDTO>>(res);
            return Ok(res1);
        }

        [HttpDelete("{candidateId}")]
        public async Task<IActionResult> DeleteCandidates(int candidateId)
        {
            await _candidatesService.DeleteCandidate(candidateId);

            return Ok();
        }

        [HttpPut("{candidateId}")]
        public async Task<IActionResult> UpdateCandidate(int candidateId,CandidateDTO candidateDTO)
        {
            var candidate = _mapper.Map<Candidate>(candidateDTO);
            await _candidatesService.UpdateCandidate(candidateId,candidate);
            return Ok();
        }
    }
}
