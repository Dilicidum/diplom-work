using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AnalysisRepository : RepositoryBase<Analysis>, IAnalysisRepository
    {
        private readonly ApplicationContext _context;

        public AnalysisRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
