using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CandidatesRepository : RepositoryBase<Candidate>, ICandidatesRepository
    {
        private readonly ApplicationContext _context;

        public CandidatesRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Candidate> GetById(int id)
        {
            var res = await _context.Candidates.Include(x=>x.CandidateCriterias).FirstOrDefaultAsync(x=>x.Id == id);
            return res;
        }

        public async Task <IEnumerable<Candidate>> GetCandidatesByVacancyId(int vacancyId)
        {
            var res = await _context.Candidates.Include(x=>x.CandidateCriterias).Where(x=>x.VacancyId == vacancyId).ToListAsync();
            return res;
        }
    }
}
