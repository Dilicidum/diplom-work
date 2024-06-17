using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository
    {
        private readonly ApplicationContext _context;

        public VacancyRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Vacancy> GetTaskByIdWithIncluded(int taskId)
        {
            var result = (await _context.Tasks.Include(x=>x.Candidates).Include(x=>x.Criterias).Include(x=>x.Analyses).FirstOrDefaultAsync(x=>x.Id == taskId));
            return result;
        } 
    }
}
